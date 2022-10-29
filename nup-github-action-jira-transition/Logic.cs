using DotNet.GitHubAction.GithubActionModels;
using DotNet.GitHubAction.JiraLogic;
using DotNet.GitHubAction.OctoStuff;
using MoreLinq;
using Newtonsoft.Json;

namespace DotNet.GitHubAction;

public class Logic
{
    private readonly ILogger _logger;
    private readonly ActionInputs _options;
    private readonly GithubActionContext_pullrequest _githubContext;
    private JiraAbstraction _jiraAbstraction;
    private GitGraph _gitGraph;
    private BranchComparer _branchComparer;

    public Logic(ILogger logger, ActionInputs options, GithubActionContext_pullrequest? githubContext)
    {
        _logger = logger;
        _options = options;
        _githubContext = githubContext;

        _jiraAbstraction = new JiraAbstraction(_logger, options.JiraUrl, options.JiraUser, options.JiraApiKey);
        var repo = githubContext.Repository.Split("/").Last();
        _gitGraph = new GitGraph(githubContext.RepositoryOwner, githubContext.Token,
            repo);
        _branchComparer = new BranchComparer(githubContext.Token, githubContext.RepositoryOwner, repo,
            _githubContext.Ref.Split("/").Last());
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
                $"Found the following Ids in commit messages: {JsonConvert.SerializeObject(deriveTicketRevertstate.Select(x=>x.Id).ToList(), Formatting.Indented)}");
            // find ids in jira
            var jiraIssues = await _jiraAbstraction.findJiraIssues(deriveTicketRevertstate.Select(x=>x.Id).ToArray());
            _logger.LogInformation(
                $"Found the following Ids in Jira: {JsonConvert.SerializeObject(jiraIssues.Select(x => x.Key), Formatting.Indented)}");

            var tickets = deriveTicketRevertstate.Where(x=>jiraIssues.Keys.Contains(x.Id.ToUpperInvariant())).ToList();
            // transistion
            var tasks = tickets.Select(async x => await _jiraAbstraction.TransistionIssue(x.Id,
                DetermineTransition(x), executionContext)).ToList();

            Task.WaitAll(tasks.ToArray());
        }
    }

    private string DetermineTransition(TicketState x)
    {
        return x.IsReverted?_options.jira_state_when_revert: _githubContext.BaseRef.ToLowerInvariant() == "main"
            ? _options.main_jira_transition
            : _options.release_jira_transition;
    }

    public static List<TicketState> DeriveTicketRevertstate(List<string> msgs)
    {
        var c = msgs.Select(x => new { ids = JiraIssueStringSearcher.FindIds(x), msg = x })
            .SelectMany(c => c.ids.Select(xx => new TicketState
            {
                Id = xx,
                Msg = c.msg
            })).GroupBy(x=>x.Id).ToList();

        var ticketStates = c.Select(x=>
        {
            var state = new TicketState();
            state.Id = x.Key;
            state.Msg = x.First().Msg;
            var reverts = x.Sum(z=>
                z.Msg.ToLowerInvariant().Split("revert").Count()-1
            );
            var ignore_auto_generated_gitkraken = x.Sum(z=>
                z.Msg.ToLowerInvariant().Split("this reverts commit").Count()-1
            );
            state.IsReverted = (reverts-ignore_auto_generated_gitkraken)%2==1;
            return state;
        });
        return ticketStates.ToList();
    }

    public class TicketState
    {
        public string Id { get; set; }
        public bool IsReverted { get; set; }
        public string Msg { get; set; }
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
            default:
                break;
        }

        _logger.LogInformation($"DeriveContext for event trigger {eventName} - determined context: {e}");

        return e;
    }


    private async Task<List<string>> GetCommitMessages(ExecutionContext executionContext)
    {
        switch (executionContext)
        {
            case ExecutionContext.PullRequest:
                return (await _gitGraph.listCommitMessagesInPullRequest((int)_githubContext.Event.Number, ""))
                    .Select(x => x.Message).ToList();
            case ExecutionContext.Push:
                return await _branchComparer.Compare(_options.branch_to_compare_to);
                break;
            default:
            {
                _logger.LogInformation($"No messages retrieved, due to unsupported event trigger {executionContext}");
                return new List<string>();
            }
        }
    }

    private static IEnumerable<string> FindIssueKeys(List<string> msgs)
    {
        return msgs.Where(c => !string.IsNullOrWhiteSpace(c))
            .SelectMany(x => JiraIssueStringSearcher.FindIds(x));
    }
}