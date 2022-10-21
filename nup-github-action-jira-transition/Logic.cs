using Newtonsoft.Json;

namespace DotNet.GitHubAction;

public class Logic
{
    private readonly ILogger _logger;

    public Logic(ILogger logger)
    {
        _logger = logger;
    }

    public void DoDaThing(ActionInputs options)
    {
        if (options.From =="text")
        {
            var ids = JiraIssueStringSearcher.FindIds(options.Text);
            _logger.LogInformation($"ids found from text: {JsonConvert.SerializeObject(ids,Formatting.Indented)}");     
        }
    }
}