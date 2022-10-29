using NUnit.Framework;

namespace tests;

public class HandleRevertTests
{
    [Test]
    public async Task Reverted()
    {
        new List<string>
        {
@"Revert ""Revert ""Merge pull request #2165 from AUTOProff/DEV-877-fe-create-form-containing-gp-options-to-dev-new""

This reverts commit 064f44c81c6e05d5bf845c6936b3822036e5da48.
6158b5"
        };
    }
}