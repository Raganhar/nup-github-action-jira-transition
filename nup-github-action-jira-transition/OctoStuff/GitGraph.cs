using Newtonsoft.Json;
using Octokit;
using Octokit.GraphQL;
using static Octokit.GraphQL.Variable;

namespace DotNet.GitHubAction.OctoStuff;

public class GitGraph
{
    private string _repoowner;
    private string _githubToken;
    private string _repo;

    public GitGraph(string repoowner, string githubToken, string repo)
    {
        _repoowner = repoowner;
        _githubToken = githubToken;
        _repo = repo;
    }
    public async Task<List<CommitMessageInfo>> listCommitMessagesInPullRequest(int prnumber, string after)
    {
        var connection = new Connection(new ProductHeaderValue("bob"), _githubToken);

        var query = new Query()
            .RepositoryOwner(_repoowner).Repository(_repo).PullRequest(prnumber)
            .Commits(100, after)
            .Nodes
            .Select(x => new CommitMessageInfo()
            {
                Name = x.Commit.Message,
                BaseRefName = x.PullRequest.BaseRefName,
                HeadRefName = x.PullRequest.HeadRefName,
            });

        var res = await connection.Run(query);
        return res.ToList();
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

    public class CommitMessageInfo
    {
        public string Name { get; set; }
        public string BaseRefName { get; set; }
        public string HeadRefName { get; set; }
    }
}