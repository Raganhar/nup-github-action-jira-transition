using System.Text.RegularExpressions;

namespace DotNet.GitHubAction;

public class JiraIssueStringSearcher
{
    // export const issueIdRegEx = ;
    private const string issueIdRegex = @"([\dA-Za-z]+-\d+)";

    public static List<string> FindIds(string text)
    {
        var matches = new Regex(issueIdRegex).Match(text);

        return matches.Groups.Values.Select(x => x.Value).Distinct().ToList();
    }
}