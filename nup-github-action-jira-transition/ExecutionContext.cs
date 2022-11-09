namespace DotNet.GitHubAction;

public enum ExecutionContext
{
    Push,
    PullRequest,
    Workflow_trigger,
    Unknown
}