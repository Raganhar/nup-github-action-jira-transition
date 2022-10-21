using DotNet.GitHubAction;
using FluentAssertions;
using NUnit.Framework;

namespace tests;

public class Class1
{
    [TestCase("NIC-6","NIC-6")]
    [TestCase("NIC-6 asdasd","NIC-6")]
    public void NAME_Test(string text, string expected)
    {
        var findIds = JiraIssueStringSearcher.FindIds(text);
        findIds.Count.Should().Be(1);
        findIds.Should().Contain(expected);
    }
}