using InventoryModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace EFCore_Library
{
    public class InventoryDbContext : DbContext
    {
        private static IConfigurationRoot _configuration;
        public DbSet<Item> Items { get; set; }
        public InventoryDbContext(DbContextOptions<InventoryDbContext> options) : base(options)
        {
        }

        public InventoryDbContext()
        {
        }
    }
}