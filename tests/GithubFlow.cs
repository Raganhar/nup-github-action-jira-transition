using DotNet.GitHubAction;
using DotNet.GitHubAction.GithubActionModels;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using NUnit.Framework;

namespace tests;

[Category("Integration")]
public class GithubFlow
{
    [Test]
    public async Task Full_flow_Test()
    {
        var context =JsonConvert.DeserializeObject<GithubActionContext_pullrequest>(File.ReadAllText("ExampleContexts/Pull_request_ExampleContext.json"));
        var credentials = Utils.GetCredentials();
        context.Token = credentials.githubToken;
        var options = new ActionInputs
        {
          From  = MagicStrings.text+"asd",
          JiraUrl = credentials.jiraUrl,
          JiraUser = credentials.JiraUser,
          JiraApiKey = credentials.JiraToken,
        };
        var logic = new Logic(NSubstitute.Substitute.For<ILogger>(), options, context);
        
        await logic.DoDaThing();
    }
}