using Atlassian.Jira;

namespace DotNet.GitHubAction.JiraLogic;

public class JiraAbstraction
{
    private readonly ILogger _logger;
    private Jira _jirClient;

    public JiraAbstraction(ILogger logger, string jiraUrl, string jiraUserid, string jiraApiKey)
    {
        _logger = logger;
        _jirClient = Jira.CreateRestClient(jiraUrl,jiraUserid,jiraApiKey);
    }

    public async Task<IDictionary<string, Issue>> findJiraIssues(params string[] issueKeys)
    {
        return await _jirClient.Issues.GetIssuesAsync(issueKeys);
    }

    public async Task<IEnumerable<IssueTransition>> Transistions(string issueId)
    {
        return await _jirClient.Issues.GetActionsAsync(issueId);
    }

    public async Task AddComment(string issueid, string comment)
    {
        await _jirClient.Issues.AddCommentAsync(issueid, new Comment { Body = comment,Author = "AP Jira Automation"});
    }

    public async Task TransistionIssue(string issueKey, string transistion, ExecutionContext executionContext)
    {
        var issue = (await findJiraIssues(issueKey)).First();
        var tra = await Transistions(issue.Key);
        var issueTransition = tra.FirstOrDefault(x=>x.Name.ToLowerInvariant() == transistion.ToLowerInvariant());
        if (issueTransition == null)
        {
            _logger.LogInformation($"Unable to transition {issueKey} to {transistion} since it doesn't exist in it's workflow");
            return;
        }
        if (issue.Value.Status.Name.ToLowerInvariant() == issueTransition.Name.ToLowerInvariant())
        {
            return;
        }
        await _jirClient.Issues.ExecuteWorkflowActionAsync(issue.Value, issueTransition.Name,new WorkflowTransitionUpdates
        {
            Comment = "no idea comment"
        });
        var executionContextExplaination = (executionContext==ExecutionContext.Push?"Branch was updated":"PR is being merged");
        var comment = $"Transitioned ticket from \"{issue.Value.Status}\" to \"{issueTransition.Name}\" due to {executionContextExplaination}";
        await AddComment(issueKey,comment);
        _logger.LogInformation(comment);
    }
}