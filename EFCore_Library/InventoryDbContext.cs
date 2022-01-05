using Microsoft.EntityFrameworkCore;

namespace EFCore_Library
{
    public class InventoryDbContext : DbContext
    {
        public InventoryDbContext(DbContextOptions options) : base(options)
        {
        }

        public InventoryDbContext()
        {
        }
    }
}