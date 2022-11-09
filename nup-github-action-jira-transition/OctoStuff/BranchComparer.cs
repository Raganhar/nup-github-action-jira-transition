using Newtonsoft.Json;
using Octokit;

namespace DotNet.GitHubAction.OctoStuff;

public class BranchComparer
{
    private GitHubClient _gitHubClient;
    private string _repoOwner;
    private string _repo;
    private string _currentBranchName;

    public BranchComparer(string token, string repoOwner, string repo, string currentBranchName)
    {
        _currentBranchName = currentBranchName;
        _repo = repo;
        _repoOwner = repoOwner;
        var github = new GitHubClient(new ProductHeaderValue("bob"));
        var tokenAuth = new Octokit.Credentials(token);
        _gitHubClient = github;
        _gitHubClient.Credentials = tokenAuth;   
    }
    public async Task<List<(string Message, string Url)>> Compare(string otherBranchHead, ILogger logger)
    {
        
        var res = await _gitHubClient.Repository.Get(_repoOwner, _repo);
        var main = await _gitHubClient.Repository.Branch.Get(res.Id, otherBranchHead);
        var other = await _gitHubClient.Repository.Branch.Get(res.Id, _currentBranchName);

        var baseCommitSha = main.Commit.Sha;
        var otherCommitSha = other.Commit.Sha;
        return await CommitDiff(baseCommitSha, otherCommitSha);
    }

    public async Task<List<(string Message, string Url)>> CommitDiff(string baseCommitSha, string otherCommitSha)
    {
        var comitdiff = await _gitHubClient.Repository.Commit.Compare(_repoOwner,
            _repo, baseCommitSha, otherCommitSha);

        return comitdiff.Commits.Select(x => (x.Commit.Message, x.Url)).ToList();
    }
}