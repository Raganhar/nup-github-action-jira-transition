using DotNet.GitHubAction.GithubActionModels;
using DotNet.GitHubAction.JiraLogic;
using DotNet.GitHubAction.OctoStuff;
using Newtonsoft.Json;

using IHost host = Host.CreateDefaultBuilder(args)
    // .ConfigureServices((_, services) => services.AddGitHubActionServices())
    .Build();

static TService Get<TService>(IHost host)
    where TService : notnull =>
    host.Services.GetRequiredService<TService>();

var logger = Get<ILoggerFactory>(host)
    .CreateLogger("DotNet.GitHubAction.Program");
var parser = Default.ParseArguments<ActionInputs>(() => new(), args);
parser.WithNotParsed(
    errors =>
    {
        logger
            .LogError(
                string.Join(Environment.NewLine, errors.Select(error => error.ToString())));
        
        Environment.Exit(2);
    });

await parser.WithParsedAsync(async options =>
{
    var environmentVariable = Environment.GetEnvironmentVariable("GITHUB_CONTEXT");
    var context = JsonConvert.DeserializeObject<GithubActionContext_pullrequest>(environmentVariable);

    await new Logic(logger, options, context).DoDaThing();
});
Environment.Exit(0);
await host.RunAsync();
