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
            var connectionString = configuration.GetConnectionString("InventoryDatabase");

            var optionsBuilder = new DbContextOptionsBuilder<InventoryDbContext>();
            optionsBuilder.UseSqlServer(connectionString);

            return new InventoryDbContext(optionsBuilder.Options);
        }
    }
}
