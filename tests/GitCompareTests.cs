using NUnit.Framework;
using Octokit;

namespace tests;

public class GitCompareTests
{
    [Test]
    public async Task stuff()
    {
        var credentials = Utils.GetCredentials();
        
        var github = new GitHubClient(new ProductHeaderValue("MyAmazingApp"));
        var tokenAuth = new Credentials
        {
            githubToken = credentials.githubToken
        }; // NOTE: not real token
        var res = await github.Repository.Get("Raganhar","nup-github-action-jira-transition");

        Console.WriteLine(res.Id);
        var comitdiff = await github.Repository.Commit.Compare("Raganhar","nup-github-action-jira-transition","","");
    }
}

