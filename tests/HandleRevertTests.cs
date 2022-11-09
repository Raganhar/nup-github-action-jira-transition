using DotNet.GitHubAction;
using FluentAssertions;
using NUnit.Framework;

namespace tests;

public class HandleRevertTests
{
    [Test]
    public async Task Reverted()
    {
        var commits = new List<string>
        {
            @"Revert ""Revert ""Merge pull request #2165 from AUTOProff/DEV-877-fe-create-form-containing-gp-options-to-dev-new""

This reverts commit 064f44c81c6e05d5bf845c6936b3822036e5da48.
6158b5",
            @"Revert ""bob-1 changed readme""This reverts commit 3b0b9409a13a9e49ad1cc9eca9262f33be2ff98e.",
            @"bob-1 changed readme",
            @"changed bob-2 readme",
        };

        var ticketStates = Logic.DeriveTicketRevertstate(commits.Select(x=> (x,"")).ToList());
        var doubleReverted = ticketStates.First(x => x.Id.ToLowerInvariant() == "Dev-877".ToLowerInvariant());
        var reverted = ticketStates.First(x => x.Id.ToLowerInvariant() == "bob-1".ToLowerInvariant());
        ticketStates.First(x => x.Id.ToLowerInvariant() == "bob-2".ToLowerInvariant()).IsReverted.Should().BeFalse();

        reverted.IsReverted.Should().BeTrue();
        doubleReverted.IsReverted.Should().BeFalse();
    }


    [Test]
    public async Task Reverteds()
    {
        var commits = new List<string>
        {
            "done with stuf",
            "Merge branch 'bob-2' into release",
            "Revert \"Merge branch 'bob-2' into release\"\n\nThis reverts commit ef8493c93d406f9ab65e75a3af8f6e015cc43702.",
            "Revert \"Revert \"Merge branch 'bob-2' into release\"\"\n\nThis reverts commit a89bf1a835deefa6c9f4a9a3e487d9eb0633ef09."
        };

        var ticketStates = Logic.DeriveTicketRevertstate(commits.Select(x=> (x,"")).ToList());
        ticketStates.First(x => x.Id.ToLowerInvariant() == "bob-2".ToLowerInvariant())
            .IsReverted.Should().BeFalse();
    }
}