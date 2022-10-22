using Newtonsoft.Json;

namespace tests;

public static class Utils
{
    public static Credentials GetCredentials()
    {
        var readAllText = File.ReadAllText("Credentials.txt");
        return JsonConvert.DeserializeObject<Credentials>(readAllText);
    }
}