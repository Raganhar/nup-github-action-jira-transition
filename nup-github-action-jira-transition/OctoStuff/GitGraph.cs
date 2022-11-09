using DotNet.GitHubAction.OctoStuff.OctoKitAutoModels;
using Newtonsoft.Json;
using Octokit;
using Octokit.GraphQL;
using static Octokit.GraphQL.Variable;
using Connection = Octokit.GraphQL.Connection;
using ProductHeaderValue = Octokit.GraphQL.ProductHeaderValue;

namespace DotNet.GitHubAction.OctoStuff;

public class GitGraph
{
    private string _repoowner;
    private string _githubToken;
    private string _repo;
    private readonly ILogger _logger;

    public GitGraph(string repoowner, string githubToken, string repo, ILogger logger)
    {
        _repoowner = repoowner;
        _githubToken = githubToken;
        _repo = repo;
        _logger = logger;
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
                Message = x.Commit.Message,
                BaseRefName = x.PullRequest.BaseRefName,
                HeadRefName = x.PullRequest.HeadRefName,
                PullRequestUrl = x.PullRequest.Url
            });

        var res = await connection.Run(query);
        return res.ToList();
    }
    
    public async Task<List<(string Message, string Url)>> bob(string branchName)
    {
        var connection = new Connection(new ProductHeaderValue("bob"), _githubToken);
        var querystring = @"{
  repository(owner: ""raganhar"", name: ""nup-github-action-jira-transition"") {
    ref (qualifiedName:""main"") {
          ... on Ref {
            name
            target {
              ... on Commit {
                history(first: 1) {
                  edges {
                    node {
                      ... on Commit {
                        committedDate
                        message
                        oid
                        url
                      }
                    }
                  }
                }
            }
          }
      }
    }
  }
}";
        var res = await connection.Run(new TextQuery(querystring).ToString());
        var data = JsonConvert.DeserializeObject<root>(res);
        
        var lastCommit = data.Data.Repository.Ref.Target.History.Edges.First();
        return new List<(string Message, string Url)>
        {
            new (lastCommit.Node.Message, lastCommit.Node.Url)
        };
    }

    public class CommitMessageInfo
    {
        public string Message { get; set; }
        public string BaseRefName { get; set; }
        public string HeadRefName { get; set; }
        public string PullRequestUrl { get; set; }
    }
    public class TextQuery 
    {
        private readonly string _queryText;
        private readonly Dictionary<string, object> _variables;

        public TextQuery(string queryText, Dictionary<string, object> variables = null)
        {
            _queryText = queryText;
            _variables = variables;
        }

        public override string ToString()
        {
            var query = new
            {
                query = _queryText,
                _variables = _variables
            };

            var json = JsonConvert.SerializeObject(query);
            return json;
        }

    }
}