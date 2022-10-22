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

    public Logic(ILogger logger, ActionInputs options, GithubActionContext_pullrequest? githubContext)
    {
        _logger = logger;
        _options = options;
        _githubContext = githubContext;

        _jiraAbstraction = new JiraAbstraction(_logger,options.JiraUrl, options.JiraUser, options.JiraApiKey);
        _gitGraph = new GitGraph(githubContext.RepositoryOwner, githubContext.Token,
            githubContext.Repository.Split("/").Last());
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
            var msgs = await _gitGraph.listCommitMessagesInPullRequest((int)_githubContext.Event.Number, "");
            _logger.LogInformation($"Found the following commit messages: {JsonConvert.SerializeObject(msgs,Formatting.Indented)}");
            // find ids
            var ids = FindIssueKeys(msgs);
            _logger.LogInformation($"Found the following Ids in commit messages: {JsonConvert.SerializeObject(ids,Formatting.Indented)}");
            // find ids in jira
            var jiraIssues = await _jiraAbstraction.findJiraIssues(ids.ToArray());
            _logger.LogInformation($"Found the following Ids in Jira: {JsonConvert.SerializeObject(jiraIssues.Select(x=>x.Key),Formatting.Indented)}");
            // transistion
            var tasks = jiraIssues.Select(async x => await _jiraAbstraction.TransistionIssue(x.Key,_githubContext.BaseRef.ToLowerInvariant() == "main"?_options.main_jira_transition:_options.release_jira_transition)).ToList();

            Task.WaitAll(tasks.ToArray());
        }
    }

    private static IEnumerable<string> FindIssueKeys(List<GitGraph.CommitMessageInfo> msgs)
    {
        return msgs.Where(c => !string.IsNullOrWhiteSpace(c.Message))
            .SelectMany(x => JiraIssueStringSearcher.FindIds(x.Message));
    }
}