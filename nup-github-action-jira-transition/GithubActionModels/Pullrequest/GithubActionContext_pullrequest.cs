using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

public partial class GithubActionContext_pullrequest
{
    [JsonProperty("token")] public string Token { get; set; }

    [JsonProperty("job")] public string Job { get; set; }

    [JsonProperty("ref")] public string Ref { get; set; }

    [JsonProperty("sha")] public string Sha { get; set; }

    [JsonProperty("repository")] public string Repository { get; set; }

    [JsonProperty("repository_owner")] public string RepositoryOwner { get; set; }

    [JsonProperty("repository_owner_id")]
    [JsonConverter(typeof(ParseStringConverter))]
    public long RepositoryOwnerId { get; set; }

    [JsonProperty("repositoryUrl")] public string RepositoryUrl { get; set; }

    [JsonProperty("run_id")] public string RunId { get; set; }

    [JsonProperty("run_number")]
    [JsonConverter(typeof(ParseStringConverter))]
    public long RunNumber { get; set; }

    [JsonProperty("retention_days")]
    [JsonConverter(typeof(ParseStringConverter))]
    public long RetentionDays { get; set; }

    [JsonProperty("run_attempt")]
    [JsonConverter(typeof(ParseStringConverter))]
    public long RunAttempt { get; set; }

    [JsonProperty("artifact_cache_size_limit")]
    [JsonConverter(typeof(ParseStringConverter))]
    public long ArtifactCacheSizeLimit { get; set; }

    [JsonProperty("repository_visibility")]
    public string RepositoryVisibility { get; set; }

    [JsonProperty("repository_id")]
    [JsonConverter(typeof(ParseStringConverter))]
    public long RepositoryId { get; set; }

    [JsonProperty("actor_id")]
    [JsonConverter(typeof(ParseStringConverter))]
    public long ActorId { get; set; }

    [JsonProperty("actor")] public string Actor { get; set; }

    [JsonProperty("triggering_actor")] public string TriggeringActor { get; set; }

    [JsonProperty("workflow")] public string Workflow { get; set; }

    [JsonProperty("head_ref")] public string HeadRef { get; set; }

    [JsonProperty("base_ref")] public string BaseRef { get; set; }

    [JsonProperty("event_name")] public string EventName { get; set; }

    [JsonProperty("event")] public Event Event { get; set; }

    [JsonProperty("server_url")] public Uri ServerUrl { get; set; }

    [JsonProperty("api_url")] public Uri ApiUrl { get; set; }

    [JsonProperty("graphql_url")] public Uri GraphqlUrl { get; set; }

    [JsonProperty("ref_name")] public string RefName { get; set; }

    [JsonProperty("ref_protected")] public bool RefProtected { get; set; }

    [JsonProperty("ref_type")] public string RefType { get; set; }

    [JsonProperty("secret_source")] public string SecretSource { get; set; }

    [JsonProperty("workspace")] public string Workspace { get; set; }

    [JsonProperty("action")] public string Action { get; set; }
}

public partial class Event
{
    [JsonProperty("action")] public string Action { get; set; }

    [JsonProperty("after")] public string After { get; set; }

    [JsonProperty("before")] public string Before { get; set; }

    [JsonProperty("number")] public long Number { get; set; }

    [JsonProperty("pull_request")] public PullRequest PullRequest { get; set; }

    [JsonProperty("sender")] public Sender Sender { get; set; }
}

public partial class PullRequest
{
    [JsonProperty("_links")] public Links Links { get; set; }

    [JsonProperty("active_lock_reason")] public object ActiveLockReason { get; set; }

    [JsonProperty("additions")] public long Additions { get; set; }

    [JsonProperty("milestones_url")] public string MilestonesUrl { get; set; }

    [JsonProperty("mirror_url")] public object MirrorUrl { get; set; }

    [JsonProperty("name")] public string Name { get; set; }

    [JsonProperty("node_id")] public string NodeId { get; set; }

    [JsonProperty("notifications_url")] public string NotificationsUrl { get; set; }

    [JsonProperty("open_issues")] public long OpenIssues { get; set; }

    [JsonProperty("open_issues_count")] public long OpenIssuesCount { get; set; }

    [JsonProperty("owner")] public Sender Owner { get; set; }

    [JsonProperty("private")] public bool Private { get; set; }

    [JsonProperty("pulls_url")] public string PullsUrl { get; set; }

    [JsonProperty("pushed_at")] public DateTimeOffset PushedAt { get; set; }

    [JsonProperty("releases_url")] public string ReleasesUrl { get; set; }

    [JsonProperty("size")] public long Size { get; set; }

    [JsonProperty("ssh_url")] public string SshUrl { get; set; }

    [JsonProperty("stargazers_count")] public long StargazersCount { get; set; }

    [JsonProperty("stargazers_url")] public Uri StargazersUrl { get; set; }

    [JsonProperty("statuses_url")] public string StatusesUrl { get; set; }

    [JsonProperty("subscribers_url")] public Uri SubscribersUrl { get; set; }

    [JsonProperty("subscription_url")] public Uri SubscriptionUrl { get; set; }

    [JsonProperty("svn_url")] public Uri SvnUrl { get; set; }

    [JsonProperty("tags_url")] public Uri TagsUrl { get; set; }

    [JsonProperty("teams_url")] public Uri TeamsUrl { get; set; }

    [JsonProperty("topics")] public object[] Topics { get; set; }

    [JsonProperty("trees_url")] public string TreesUrl { get; set; }

    [JsonProperty("updated_at")] public DateTimeOffset UpdatedAt { get; set; }

    [JsonProperty("url")] public Uri Url { get; set; }

    [JsonProperty("visibility")] public string Visibility { get; set; }

    [JsonProperty("watchers")] public long Watchers { get; set; }

    [JsonProperty("watchers_count")] public long WatchersCount { get; set; }

    [JsonProperty("web_commit_signoff_required")]
    public bool WebCommitSignoffRequired { get; set; }
}

public partial class Links
{
    [JsonProperty("comments")] public Comments Comments { get; set; }

    [JsonProperty("commits")] public Comments Commits { get; set; }

    [JsonProperty("html")] public Comments Html { get; set; }

    [JsonProperty("issue")] public Comments Issue { get; set; }

    [JsonProperty("review_comment")] public Comments ReviewComment { get; set; }

    [JsonProperty("review_comments")] public Comments ReviewComments { get; set; }

    [JsonProperty("self")] public Comments Self { get; set; }

    [JsonProperty("statuses")] public Comments Statuses { get; set; }
}

public partial class Comments
{
    [JsonProperty("href")] public string Href { get; set; }
}

public partial class Sender
{
    [JsonProperty("avatar_url")] public Uri AvatarUrl { get; set; }

    [JsonProperty("events_url")] public string EventsUrl { get; set; }

    [JsonProperty("followers_url")] public Uri FollowersUrl { get; set; }

    [JsonProperty("following_url")] public string FollowingUrl { get; set; }

    [JsonProperty("gists_url")] public string GistsUrl { get; set; }

    [JsonProperty("gravatar_id")] public string GravatarId { get; set; }

    [JsonProperty("html_url")] public Uri HtmlUrl { get; set; }

    [JsonProperty("id")] public long Id { get; set; }

    [JsonProperty("login")] public string Login { get; set; }

    [JsonProperty("node_id")] public string NodeId { get; set; }

    [JsonProperty("organizations_url")] public Uri OrganizationsUrl { get; set; }

    [JsonProperty("received_events_url")] public Uri ReceivedEventsUrl { get; set; }

    [JsonProperty("repos_url")] public Uri ReposUrl { get; set; }

    [JsonProperty("site_admin")] public bool SiteAdmin { get; set; }

    [JsonProperty("starred_url")] public string StarredUrl { get; set; }

    [JsonProperty("subscriptions_url")] public Uri SubscriptionsUrl { get; set; }

    [JsonProperty("type")] public string Type { get; set; }

    [JsonProperty("url")] public Uri Url { get; set; }
}

internal class ParseStringConverter : JsonConverter
{
    public override bool CanConvert(Type t) => t == typeof(long) || t == typeof(long?);

    public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
    {
        if (reader.TokenType == JsonToken.Null) return null;
        var value = serializer.Deserialize<string>(reader);
        long l;
        if (Int64.TryParse(value, out l))
        {
            return l;
        }

        throw new Exception("Cannot unmarshal type long");
    }

    public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
    {
        if (untypedValue == null)
        {
            serializer.Serialize(writer, null);
            return;
        }

        var value = (long)untypedValue;
        serializer.Serialize(writer, value.ToString());
        return;
    }

    public static readonly ParseStringConverter Singleton = new ParseStringConverter();
}