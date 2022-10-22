﻿using DotNet.GitHubAction.JiraLogic;
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
    new Logic(logger, new JiraAbstraction(options.JiraUrl,options.JiraUser,options.JiraApiKey)).DoDaThing(options);
    Task.Delay(1000);
    return Task.CompletedTask;
});
Environment.Exit(0);
await host.RunAsync();
