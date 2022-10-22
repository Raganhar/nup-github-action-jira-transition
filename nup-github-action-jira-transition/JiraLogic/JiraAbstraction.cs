﻿using Atlassian.Jira;

namespace DotNet.GitHubAction.JiraLogic;

public class JiraAbstraction
{
    private Jira _jirClient;

    public JiraAbstraction(string jiraUrl, string jiraUserid, string jiraApiKey)
    {
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

    public async Task TransistionIssue(string issueKey, string transistion)
    {
        var jiraIssues = (await findJiraIssues(issueKey)).First();
        var tra = await Transistions(jiraIssues.Key);
        var issueTransition = tra.First(x=>x.Name.ToLowerInvariant() == transistion.ToLowerInvariant());
        await _jirClient.Issues.ExecuteWorkflowActionAsync(jiraIssues.Value, issueTransition.Name,new WorkflowTransitionUpdates
        {
            Comment = "no idea comment"
        });
        await AddComment(issueKey,$"Transitioned ticket from {jiraIssues.Value.Status} to {issueTransition.Name} due to X");
    }
}