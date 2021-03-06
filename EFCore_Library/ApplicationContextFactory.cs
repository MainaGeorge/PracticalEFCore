using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace EFCore_Library
{
    public class ApplicationContextFactory : IDesignTimeDbContextFactory<InventoryDbContext>
    {
        public InventoryDbContext CreateDbContext(string[] args)
        {
            var builder = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json", reloadOnChange: true, optional: true);
            var configuration = builder.Build();
            var connectionString = configuration.GetConnectionString("LocalInventoryDatabase");


            var optionsBuilder = new DbContextOptionsBuilder<InventoryDbContext>();
            optionsBuilder.UseSqlServer(connectionString, settings =>
            {
                settings.EnableRetryOnFailure();
                settings.CommandTimeout(15);
            });

            return new InventoryDbContext(optionsBuilder.Options);
        }
    }
}
