using ActivityThree;
using Microsoft.Extensions.Configuration;

static class Program
{
    private static IConfigurationRoot? configurationRoot;
    static void Main()
    {
        BuildOptions();
        Console.WriteLine(configurationRoot?.GetConnectionString("AdventureWorks"));
    }

    static void BuildOptions()
    {
        configurationRoot = ConfigurationBuilderSingleton.ConfigurationRoot;
    }
}

