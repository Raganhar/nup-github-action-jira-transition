using DotNet.GitHubAction.OctoStuff;
using Newtonsoft.Json;
using NUnit.Framework;

namespace tests;

[Category("Integration")]
public class OctoTests
{
    private static GitGraph _gitGraph;

    [Test]
    public async Task CommitMessages_in_pr_Test()
    {
        var msgs = await _gitGraph.listCommitMessagesInPullRequest(1,null);
        Console.WriteLine(JsonConvert.SerializeObject(msgs, Formatting.Indented));
    }

    private static GitGraph Setup()
    {
        var gitGraph = _gitGraph;
        return gitGraph;
    }

    [Test]
    public async Task NAME_Test()
    {
        _gitGraph = new GitGraph("", "", "");
    }
}