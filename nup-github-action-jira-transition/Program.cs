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

await parser.WithParsedAsync(options =>
{
    logger.LogInformation($"hello");
    var environmentVariable = Environment.GetEnvironmentVariable("GITHUB_CONTEXT");
    var context = JsonConvert.DeserializeObject<GithubActionContext_pullrequest>(environmentVariable);
    logger.LogInformation($"options: {JsonConvert.SerializeObject(options, Formatting.Indented)}");

    new Logic(logger, options, context).DoDaThing();
    Task.Delay(10000);
    return Task.CompletedTask;
});
Environment.Exit(0);
await host.RunAsync();
