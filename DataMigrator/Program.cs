
using System.ComponentModel;
using EFCore_Library;
using InventoryHelper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
#nullable disable

namespace DataMigrator
{

    internal static class Program
    {
        private static IConfigurationRoot _configurationRoot;
        private static DbContextOptionsBuilder<InventoryDbContext> _optionsBuilder;
        private static void Main()
        {
            BuildOptions();
            ApplyMigrations();
            ExecuteCustomSeedData();
        }

        private static void BuildOptions()
        {
            _configurationRoot = ConfigurationBuilderSingleton.ConfigurationRoot;
            _optionsBuilder = new DbContextOptionsBuilder<InventoryDbContext>();
            _optionsBuilder.UseSqlServer(_configurationRoot.GetConnectionString("LocalInventoryDatabase"), settings =>
            {
                settings.EnableRetryOnFailure();
                settings.CommandTimeout(10);
            });
        }

        private static void ApplyMigrations()
        {
            using var db = new InventoryDbContext(_optionsBuilder.Options);

            db.Database.Migrate();
        }

        private static void ExecuteCustomSeedData()
        {
            using var context = new InventoryDbContext(_optionsBuilder.Options);
            var categories = new BuildCategories(context);
            var items = new BuildItems(context);
            categories.ExecuteSeed();
            items.ExecuteSeed();
        }
    }
}
