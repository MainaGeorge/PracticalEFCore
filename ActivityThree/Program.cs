using EFCore_Library;
using EFCore_Library.Migrations.Scripts;
using InventoryHelper;
using InventoryModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Migrations.Operations;
using Microsoft.EntityFrameworkCore.Migrations.Operations.Builders;
using Microsoft.Extensions.Configuration;
using System.Reflection;
using System.Text;

static class Program
{
    private static IConfigurationRoot configurationRoot;
    private static DbContextOptionsBuilder<InventoryDbContext> _optionsBuilder;

    //simulate a user in the system
    private const string _systemUserId = "2fd28110-93d0-427d-9207-d55dbca680fa";

    //simulate a logged in user who is making changes to the db
    private const string _loggedInUserId = "e2eb8989-a81a-4151-8e86-eb95a7961da2";

    static void Main()
    {
        BuildOptions();
        DeleteAllItems();
        EnsureItems();
        ListInventory();
        GetItemsForListing();
    }

    private static void DeleteAllItems()
    {
        using var db = new InventoryDbContext(_optionsBuilder.Options);
        var items = db.Items.ToList();

        db.Items.RemoveRange(items);

        db.SaveChanges();
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
        var existingItem =  db.Items.FirstOrDefault(x => x.Name.ToLower() == name.ToLower());

        if(existingItem is null)
        {
            var item = new Item 
            { 
                Name = name,
                Description=description, 
                Notes=notes, 
                IsActive=true, 
                CreateByUserId=_loggedInUserId, 
                Quantity=random.Next(1, 1000) 
            };

            db.Items.Add(item);
            db.SaveChanges();
        }
    }

    static void ListInventory()
    {
        using var db = new InventoryDbContext(_optionsBuilder.Options);
        var items = db.Items.OrderByDescending(i => i.Name).ToList();

        items?.ForEach(i => Console.WriteLine($"Item : {i.Name}"));
    }

    static void UpdateItems()
    {
        using var db = new InventoryDbContext(_optionsBuilder.Options);
        var items = db.Items.ToList();

        items.ForEach(i => i.CurrentOrFinalPrice = 9.99M);
        db.Items.UpdateRange(items);

        db.SaveChanges();
        
    }

    static void GetItemsForListing()
    {
        using var db = new InventoryDbContext(_optionsBuilder.Options);
        var results = db.ItemsForListing.FromSqlRaw("EXECUTE dbo.GetItemsForListing").ToList();

        results.ForEach(i => Console.WriteLine($"{i.Name} {i?.CategoryName}"));

    }
}

