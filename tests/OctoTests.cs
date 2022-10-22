using DotNet.GitHubAction.OctoStuff;
using NUnit.Framework;

namespace tests;

public class OctoTests
{
    [Test]
    public async Task CommitMessages_in_pr_Test()
    {
        await new GitGraph().listCommitMessagesInPullRequest("Raganhar", Utils.GetCredentials().githubToken, );
    }
    [Test]
    public async Task NAME_Test()
    {
        await new GitGraph().doStuff("Raganhar", Utils.GetCredentials().githubToken);
    }
}