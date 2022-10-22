using Newtonsoft.Json;
using Octokit;
using Octokit.GraphQL;
using static Octokit.GraphQL.Variable;

namespace DotNet.GitHubAction.OctoStuff;

public class GitGraph
{
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