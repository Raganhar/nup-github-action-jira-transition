using Atlassian.Jira;
using DotNet.GitHubAction.JiraLogic;
using FluentAssertions;
using Newtonsoft.Json;
using NUnit.Framework;

namespace tests;

[Category("Integration")]
public class JiraTests
{
    private static Jira? _client;
    private string _issueKey;
    private JiraAbstraction _jiraAbstraction;

    [Test]
    public async Task login_jira_Test()
    {
        var issueAsync = await _client.Projects.GetProjectsAsync();
        issueAsync.Any().Should().BeTrue();
    }

    [Test]
    public async Task find_jira_issue_Test()
    {
        var issueAsync = await _client.Issues.GetIssueAsync(_issueKey);
        issueAsync.Key.Should().Be(_issueKey);
    }
    
    [Test]
    public async Task transition_issue_Test()
    {
        var i = (await _jiraAbstraction.findJiraIssues(_issueKey)).First();
        Console.WriteLine(i.Value.Status.Name);
        await _jiraAbstraction.TransistionIssue(_issueKey,"in progress");
        i = (await _jiraAbstraction.findJiraIssues(_issueKey)).First();
        Console.WriteLine(i.Value.Status.Name);
    }

    [Test]
    public async Task Transition_list_ticket()
    {
        var i = await _client.Issues.GetIssueAsync(_issueKey);
        var res = await _client.Issues.GetActionsAsync(i.Key.Value);

        var transistions = await _jiraAbstraction.Transistions(i.Key.Value);
        res.Select(x => x.Id).ToList().Should().BeEquivalentTo(transistions.Select(c => c.Id).ToList());
        Console.WriteLine(JsonConvert.SerializeObject(transistions,Formatting.Indented));
    }

    [SetUp]
    public void setup()
    {
        _issueKey = "BOB-1";
        var apikey = "ir0QjIR9YzUS9lb4wb4qB0BE";
        var jiraurl = "https://trial-janus.atlassian.net/";
        var jiraUser = "januspeis@gmail.com";
        _client = Jira.CreateRestClient(jiraurl, jiraUser, apikey);
        _jiraAbstraction = new JiraAbstraction(jiraurl, jiraUser, apikey);
    }
}