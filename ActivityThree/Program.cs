using EFCore_Library;
using InventoryHelper;
using InventoryModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

static class Program
{
    private static IConfigurationRoot configurationRoot;
    private static DbContextOptionsBuilder<InventoryDbContext> _optionsBuilder;
    static void Main()
    {
        BuildOptions();
        EnsureItems();
        ListInventory();
    }

    static void BuildOptions()
    {
        configurationRoot = ConfigurationBuilderSingleton.ConfigurationRoot;
        _optionsBuilder = new DbContextOptionsBuilder<InventoryDbContext>();

        _optionsBuilder.UseSqlServer(configurationRoot
            .GetConnectionString("InventoryDatabase"));
    }
    static void EnsureItems()
    {
        EnsureItem("Batman Begins");
        EnsureItem("Inception");
        EnsureItem("The Matrix");
        EnsureItem("The Matrix Revolutions");
        EnsureItem("Top Gun");
    }
    private static void EnsureItem(string name)
    {
        using var db = new InventoryDbContext(_optionsBuilder.Options);
        var existingItem =  db.Items.FirstOrDefault(x => x.Name.ToLower() == name.ToLower());

        if(existingItem is null)
        {
            var item = new Item { Name = name };
            db.Items.Add(item);
            db.SaveChanges();
        }
    }

    static void ListInventory()
    {
        using var db = new InventoryDbContext(_optionsBuilder.Options);
        var items = db.Items.OrderByDescending(i => i.Name).ToList();

        items.ForEach(i => Console.WriteLine($"Item : {i.Name}"));
    }
}

