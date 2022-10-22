using DotNet.GitHubAction.GithubActionModels;
using DotNet.GitHubAction.JiraLogic;
using DotNet.GitHubAction.OctoStuff;
using Newtonsoft.Json;

namespace DotNet.GitHubAction;

public class Logic
{
    private readonly ILogger _logger;
    private readonly ActionInputs _options;
    private readonly GithubActionContext_push _contextPush;
    private JiraAbstraction _jiraAbstraction;
    private GitGraph _gitGraph;

    public Logic(ILogger logger, ActionInputs options, GithubActionContext_push contextPush)
    {
        _logger = logger;
        _options = options;
        _contextPush = contextPush;

        _jiraAbstraction = new JiraAbstraction(options.JiraUrl,options.JiraUser,options.JiraApiKey);
        _gitGraph = new GitGraph(contextPush.RepositoryOwner, contextPush.Token,contextPush.Repository);
    }

    public void DoDaThing()
    {
        if (_options.From =="text")
        {
            var ids = JiraIssueStringSearcher.FindIds(_options.Text);
            _logger.LogInformation($"ids found from text: {JsonConvert.SerializeObject(ids,Formatting.Indented)}");     
        }
        else
        {
            // _gitGraph.listCommitMessagesInPullRequest(_context.Event.)
            // find ids
            // find ids in jira
            // transistion
            _logger.LogInformation($"Should get jira IDs from git");   
        }
    }
}