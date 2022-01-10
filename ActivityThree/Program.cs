using System.Threading.Channels;
using EFCore_Library;
using InventoryHelper;
using InventoryModels;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace ActivityThree;

internal static class Program
{
    private static IConfigurationRoot _configurationRoot;
    private static DbContextOptionsBuilder<InventoryDbContext> _optionsBuilder;

    //simulate a user in the system
    private const string SYSTEM_USER_ID = "2fd28110-93d0-427d-9207-d55dbca680fa";

    //simulate a logged in user who is making changes to the db
    private const string LOGGED_IN_USER_ID = "e2eb8989-a81a-4151-8e86-eb95a7961da2";

    private static void Main()
    {
        BuildOptions();
        EnsureItems();
        ListInventory();
        GetItemsForListing();
        GetItemsDelimitedString();
        GetItemsTotalValues();
    }

    private static void DeleteAllItems()
    {
        using var db = new InventoryDbContext(_optionsBuilder.Options);
        var items = db.Items.ToList();

        db.Items.RemoveRange(items);

        db.SaveChanges();
    }

    private static void BuildOptions()
    {
        _configurationRoot = ConfigurationBuilderSingleton.ConfigurationRoot;
        _optionsBuilder = new DbContextOptionsBuilder<InventoryDbContext>();

        _optionsBuilder.UseSqlServer(_configurationRoot
            .GetConnectionString("LocalInventoryDatabase"));
    }

    private static void EnsureItems()
    {
        EnsureItem("Batman Begins", "You either die the hero or live long enough to see yourself become the villain", "Christian Bale, Katie Holmes");
        EnsureItem("Inception", "You mustn't be afraid to dream a little bigger, darling", "Leonardo DiCaprio, Tom Hardy, Joseph GordonLevitt");
        EnsureItem("Remember the Titans", "Left Side, Strong Side", "Denzell Washington, Will Patton");
        EnsureItem("Star Wars: The Empire Strikes Back", "He will join us or die, master", "Harrison Ford, Carrie Fisher, Mark Hamill");
        EnsureItem("Top Gun", "I feel the need, the need for speed!", "Tom Cruise, Anthony Edwards, Val Kilmer");
    }

    private static void EnsureItem(string name, string description, string notes)
    {
        var random = new Random();
        using var db = new InventoryDbContext(_optionsBuilder.Options);

        // ReSharper disable once SpecifyStringComparison
        var existingItem =  db.Items.FirstOrDefault(x => x.Name.ToLower() == name.ToLower());

        if (existingItem is not null) return;

        var item = new Item 
        { 
            Name = name,
            Description=description, 
            Notes=notes, 
            IsActive=true, 
            CreateByUserId=LOGGED_IN_USER_ID, 
            Quantity=random.Next(1, 1000) 
        };

        db.Items.Add(item);
        db.SaveChanges();
    }

    private static void ListInventory()
    {
        using var db = new InventoryDbContext(_optionsBuilder.Options);
        var items = db.Items.OrderByDescending(i => i.Name).ToList();

        items?.ForEach(i => Console.WriteLine($"Item : {i.Name}"));
    }

    private static void UpdateItems()
    {
        using var db = new InventoryDbContext(_optionsBuilder.Options);
        var items = db.Items.ToList();

        items.ForEach(i => i.CurrentOrFinalPrice = 9.99M);
        db.Items.UpdateRange(items);

        db.SaveChanges();
        
    }

    private static void GetItemsForListing()
    {
        using var db = new InventoryDbContext(_optionsBuilder.Options);
        var results = db.ItemsForListing.FromSqlRaw("EXECUTE dbo.GetItemsForListing").ToList();

        results.ForEach(i => Console.WriteLine($"{i.Name} {i?.CategoryName}"));

    }

    private static void GetItemsDelimitedString()
    {
        using var db = new InventoryDbContext(_optionsBuilder.Options);
        var isActiveParameter = new SqlParameter("IsActive", 1);
        var results = db
            .AllItemsOutput
            .FromSqlRaw("SELECT [dbo].[ItemNamesPipeDelimitedString](@IsActive) AS AllItems", isActiveParameter)
            .FirstOrDefault();

        Console.WriteLine($"all active items {results?.AllItems}");
    }

    private static void GetItemsTotalValues()
    {
        using var db = new InventoryDbContext(_optionsBuilder.Options);
        var isActiveParameter = new SqlParameter("IsActive", 1);

        var results = db.GetItemsTotalValues
            .FromSqlRaw("SELECT * FROM [dbo].[GetItemsTotalValue] (@IsActive)", isActiveParameter)
            .ToList();

        results.ForEach(item => Console.WriteLine($"New Item] {item.Id, -10}" + $"|{item.Name, -50}"
        + $"|{item.Quantity, -4}" + $"|{item.TotalValue, -5}"));
    }
}