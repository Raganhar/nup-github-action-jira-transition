namespace DotNet.GitHubAction;

public class MagicStrings
{
    public const string text = nameof(text);

    public static class EventNames
    {
        public const string Push = "push";
        public const string PullRequest = "pull_request_target";
        public const string workflow_dispatch = "workflow_dispatch";
    }
}
