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
    public async Task<List<string>> Compare(string otherBranchHead)
    {
        
        var res = await _gitHubClient.Repository.Get(_repoOwner, _repo);
        var main = await _gitHubClient.Repository.Branch.Get(res.Id, otherBranchHead);
        var other = await _gitHubClient.Repository.Branch.Get(res.Id, _currentBranchName);

        var comitdiff = await _gitHubClient.Repository.Commit.Compare(_repoOwner,
            _repo, main.Commit.Sha, other.Commit.Sha);

        return comitdiff.Commits.Select(x => x.Commit.Message).ToList();
    }
}