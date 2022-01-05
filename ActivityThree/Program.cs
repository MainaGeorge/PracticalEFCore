using ActivityThree;
using EFCore_Library;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

static class Program
{
    private static IConfigurationRoot configurationRoot;
    private static DbContextOptionsBuilder<InventoryDbContext> _optionsBuilder;
    static void Main()
    {
        BuildOptions();
    }

    static void BuildOptions()
    {
        configurationRoot = ConfigurationBuilderSingleton.ConfigurationRoot;
        _optionsBuilder = new DbContextOptionsBuilder<InventoryDbContext>();

        _optionsBuilder.UseSqlServer(configurationRoot
            .GetConnectionString("AdventureWorks"));
    }
}

