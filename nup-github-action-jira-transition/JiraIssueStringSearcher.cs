using System.Text.RegularExpressions;

namespace DotNet.GitHubAction;

public class JiraIssueStringSearcher
{
    // export const issueIdRegEx = ;
    private const string issueIdRegex = @"([\dA-Za-z]+-\d+)";

    public static List<string> FindIds(string text)
    {
        text = text.ReplaceLineEndings(" ");
        var matches = new Regex(issueIdRegex).Matches(text);

        return matches.Select(c=>c.Value).Distinct().ToList();
    }
}