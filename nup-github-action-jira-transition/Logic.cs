using DotNet.GitHubAction.GithubActionModels;
using DotNet.GitHubAction.JiraLogic;
using DotNet.GitHubAction.OctoStuff;
using MoreLinq;
using Newtonsoft.Json;
using Octokit.GraphQL;

namespace DotNet.GitHubAction;

public class Logic
{
    private readonly ILogger _logger;
    private readonly ActionInputs _options;
    private readonly GithubActionContext_pullrequest _githubContext;
    private JiraAbstraction _jiraAbstraction;
    private GitGraph _gitGraph;
    private BranchComparer _branchComparer;
    private string _currentBranchName;

    public Logic(ILogger logger, ActionInputs options, GithubActionContext_pullrequest? githubContext)
    {
        _logger = logger;
        _options = options;
        _githubContext = githubContext;

        _jiraAbstraction = new JiraAbstraction(_logger, options.JiraUrl, options.JiraUser, options.JiraApiKey);
        var repo = githubContext.Repository.Split("/").Last();
        _gitGraph = new GitGraph(githubContext.RepositoryOwner, githubContext.Token,
            repo);
        _currentBranchName = _githubContext.Ref.Split("/").Last();
        _branchComparer = new BranchComparer(githubContext.Token, githubContext.RepositoryOwner, repo,
            _currentBranchName);
    }

    public async Task DoDaThing()
    {
        if (_options.From == "text")
        {
            var ids = JiraIssueStringSearcher.FindIds(_options.Text);
            _logger.LogInformation($"ids found from text: {JsonConvert.SerializeObject(ids, Formatting.Indented)}");
        }
        else
        {
            var executionContext = DeriveContext(_githubContext.EventName);
            var msgs = await GetCommitMessages(executionContext);
            _logger.LogInformation(
                $"Found the following commit messages: {JsonConvert.SerializeObject(msgs, Formatting.Indented)}");
            // find ids

            var deriveTicketRevertstate = DeriveTicketRevertstate(msgs);

            _logger.LogInformation(
                $"Found the following Ids in commit messages: {JsonConvert.SerializeObject(deriveTicketRevertstate.Select(x => x.Id).ToList(), Formatting.Indented)}");
            // find ids in jira
            var jiraIssues = await _jiraAbstraction.findJiraIssues(deriveTicketRevertstate.Select(x => x.Id).ToArray());
            _logger.LogInformation(
                $"Found the following Ids in Jira: {JsonConvert.SerializeObject(jiraIssues.Select(x => x.Key), Formatting.Indented)}");

            var tickets = deriveTicketRevertstate.GroupBy(x=>x.Id).Where(x => jiraIssues.Keys.Contains(x.Key.ToUpperInvariant()))
                .ToList();

            var guessIsLast = tickets.Select(x => x.Last()).ToList();
            
            var tasks = guessIsLast.Select(async x => await _jiraAbstraction.TransistionIssue(x.Id,
                DetermineTransition(x.IsReverted), executionContext, _currentBranchName,x.Sha)).ToList();

            Task.WaitAll(tasks.ToArray());
        }
    }


    private string DetermineTransition(bool isReverted)
    {
        return isReverted
            ? _options.jira_state_when_revert
            : _githubContext.BaseRef.ToLowerInvariant() == "main"
                ? _options.main_jira_transition
                : _options.release_jira_transition;
    }

    public static List<(string Id, string Msg, bool IsReverted, string Sha)> DeriveTicketRevertstate(List<(string Message, string Url)> msgs)
    {
        var c = msgs.Select(x => new { ids = JiraIssueStringSearcher.FindIds(x.Message), m = x })
            .SelectMany(c => c.ids.Select(xx => (
                Sha:c.m.Url,
                Id: xx,
                Msg : c.m.Message)).GroupBy(x => x.Id).ToList());

        var ticketStates = c.Select(x =>
        {
            var Msg = x.Last().Msg;
            var msglower = Msg.ToLowerInvariant();
            var reverts = msglower.Split("revert").Count() - 1;
            var ignore_auto_generated_gitkraken = msglower.Split("this reverts commit").Count() - 1;
            var isReverted = (reverts - ignore_auto_generated_gitkraken) % 2 == 1;
            return (Id: x.Key, Msg:Msg,IsReverted:isReverted, Sha:x.Last().Sha);
        });
        return ticketStates.ToList();
    }

    public ExecutionContext DeriveContext(string eventName)
    {
        ExecutionContext e = ExecutionContext.Unknown;
        switch (eventName)
        {
            case MagicStrings.EventNames.Push:
                e = ExecutionContext.Push;
                break;
            case MagicStrings.EventNames.PullRequest:
                e = ExecutionContext.PullRequest;
                break;
            case MagicStrings.EventNames.workflow_dispatch:
                e = ExecutionContext.Workflow_trigger;
                break;
            default:
                break;
        }

        _logger.LogInformation($"DeriveContext for event trigger {eventName} - determined context: {e}");

        return e;
    }


    private async Task<List<(string Message, string Url)>> GetCommitMessages(ExecutionContext executionContext)
    {
        switch (executionContext)
        {
            case ExecutionContext.PullRequest:
                return (await _gitGraph.listCommitMessagesInPullRequest((int)_githubContext.Event.Number, ""))
                    .Select(x => (x.Message, x.PullRequestUrl)).ToList();
            case ExecutionContext.Push:
            case ExecutionContext.Workflow_trigger:
                return await _branchComparer.Compare(_options.branch_to_compare_to,_logger);
                break;
            default:
            {
                _logger.LogInformation($"No messages retrieved, due to unsupported event trigger {executionContext}");
                return new List<(string Message, string Url)>();
            }
        }
    }
}