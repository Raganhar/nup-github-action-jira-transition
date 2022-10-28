using NUnit.Framework;
using Octokit;

namespace tests;

public class GitCompareTests
{
    [Test]
    [Ignore("poc")]
    public async Task stuff()
    {
        var credentials = Utils.GetCredentials();

        var github = new GitHubClient(new ProductHeaderValue("MyAmazingApp"));
        var tokenAuth = new Octokit.Credentials(credentials.githubToken);
        github.Credentials = tokenAuth;
        
        var res = await github.Repository.Get("Raganhar", "nup-github-action-jira-transition");
        var main = await github.Repository.Branch.Get(res.Id, "main");
        var other = await github.Repository.Branch.Get(res.Id, "push_Test");

        var comitdiff = await github.Repository.Commit.Compare("Raganhar",
            "nup-github-action-jira-transition", main.Commit.Sha, other.Commit.Sha);

        var msgs = comitdiff.Commits.Select(x => x.Commit.Message);
    }
}