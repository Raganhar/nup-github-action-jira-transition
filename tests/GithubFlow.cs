using DotNet.GitHubAction;
using DotNet.GitHubAction.GithubActionModels;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using NUnit.Framework;

namespace tests;

public class GithubFlow
{
    [Test]
    public void NAME_Test()
    {
        var context =JsonConvert.DeserializeObject<GithubActionContext_pullrequest>(File.ReadAllText("ExampleContexts/Pull_request_ExampleContext.json"));
        var credentials = Utils.GetCredentials();
        var options = new ActionInputs
        {
          From  = MagicStrings.text+"asd",
          JiraUrl = credentials.jiraUrl,
          JiraUser = credentials.JiraUser,
          JiraApiKey = credentials.JiraToken,
        };
        var logic = new Logic(NSubstitute.Substitute.For<ILogger>(), options, context);
        
        logic.DoDaThing();
    }
}