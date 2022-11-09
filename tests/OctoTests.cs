using DotNet.GitHubAction.OctoStuff;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using NUnit.Framework;
using Microsoft.Extensions.Logging.Console;
using Microsoft.Extensions.Logging;

namespace tests;

[Category("Integration")]
public class OctoTests
{
    private static GitGraph _gitGraph;

    [Test]
    public async Task CommitMessages_in_pr_Test()
    {
        var msgs = await _gitGraph.listCommitMessagesInPullRequest(1, null);
        Console.WriteLine(JsonConvert.SerializeObject(msgs, Formatting.Indented));
    }

    [Test]
    public async Task Get_previous_commit_Test()
    {
        var msgs = await _gitGraph.Parents("main");
        Console.WriteLine(JsonConvert.SerializeObject(msgs, Formatting.Indented));
    }

    [SetUp]
    public void setup()
    {
        var fac = LoggerFactory.Create(builder => { builder.AddConsole(); });
        _gitGraph = new GitGraph("raganhar", Utils.GetCredentials().githubToken, "nup-github-action-jira-transition",fac.CreateLogger("asd"));
    }
}