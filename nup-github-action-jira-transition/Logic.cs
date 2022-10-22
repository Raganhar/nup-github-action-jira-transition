using DotNet.GitHubAction.GithubActionModels;
using DotNet.GitHubAction.JiraLogic;
using DotNet.GitHubAction.OctoStuff;
using Newtonsoft.Json;

namespace DotNet.GitHubAction;

public class Logic
{
    private readonly ILogger _logger;
    private readonly ActionInputs _options;
    private readonly GithubActionContext _context;
    private JiraAbstraction _jiraAbstraction;
    private GitGraph _gitGraph;

    public Logic(ILogger logger, ActionInputs options, GithubActionContext context)
    {
        _logger = logger;
        _options = options;
        _context = context;

        _jiraAbstraction = new JiraAbstraction(options.JiraUrl,options.JiraUser,options.JiraApiKey);
        _gitGraph = new GitGraph(context.RepositoryOwner, context.Token,context.Repository);
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
            // find ids
            // find ids in jira
            // transistion
            _logger.LogInformation($"Should get jira IDs from git");   
        }
    }
}