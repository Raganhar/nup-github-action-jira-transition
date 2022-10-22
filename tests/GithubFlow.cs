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
        var context =JsonConvert.DeserializeObject<GithubActionContext_push>(File.ReadAllText("Push_ExampleContext.json"));
        var options = new ActionInputs
        {
          From  = MagicStrings.text+"asd"
        };
        var logic = new Logic(NSubstitute.Substitute.For<ILogger>(), options, context);
        
        logic.DoDaThing();
    }
}