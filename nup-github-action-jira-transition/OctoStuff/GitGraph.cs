using Newtonsoft.Json;
using Octokit;
using Octokit.GraphQL;
using static Octokit.GraphQL.Variable;

namespace DotNet.GitHubAction.OctoStuff;

public class GitGraph
{
    public async Task listCommitMessagesInPullRequest(string repoowner,string github_token, string repo, int prnumber, string after)
    {
        var connection = new Connection(new ProductHeaderValue("bob"), github_token);

        var query = new Query()
            .RepositoryOwner(repoowner).Repository(repo).PullRequest(prnumber)
            .Commits(100, after)
            .Nodes
            .Select(x => new
            {
                Name = x.Commit.Message,
                BaseRef = x.PullRequest.BaseRefName,
                HeadRef = x.PullRequest.HeadRefName,
            });

        var res = await connection.Run(query);
        Console.WriteLine(JsonConvert.SerializeObject(res,Formatting.Indented));
    }
    
    public async Task doStuff(string repoowner,string github_token)
    {
        var connection = new Connection(new ProductHeaderValue("bob"), github_token);

        var query = new Query()
            .RepositoryOwner(repoowner)
            .Repositories(first: 30)
            .Nodes
            .Select(x => new
            {
                Name = x.Name,
                Description = x.Description,
            });
        var vars = new Dictionary<string, object>
        {
            { "owner", "octokit" },
            { "name", "octokit.graphql.net" },
        };

        var res = await connection.Run(query);
        Console.WriteLine(JsonConvert.SerializeObject(res,Formatting.Indented));
    }
}