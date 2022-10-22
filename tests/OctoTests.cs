using DotNet.GitHubAction.OctoStuff;
using Newtonsoft.Json;
using NUnit.Framework;

namespace tests;

public class OctoTests
{
    [Test]
    public async Task CommitMessages_in_pr_Test()
    {
        var msgs = await new GitGraph().listCommitMessagesInPullRequest("Raganhar", Utils.GetCredentials().githubToken, "nup-github-action-jira-transition",1,null);
        Console.WriteLine(JsonConvert.SerializeObject(msgs, Formatting.Indented));
    }
    [Test]
    public async Task NAME_Test()
    {
        await new GitGraph().doStuff("Raganhar", Utils.GetCredentials().githubToken);
    }
}