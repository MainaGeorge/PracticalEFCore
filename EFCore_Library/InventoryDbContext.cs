using InventoryModels;
using InventoryModels.DTOs;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace EFCore_Library
{
    public class InventoryDbContext : DbContext
    {
        private static IConfigurationRoot _configuration;

        //simulate a user in the system
        private const string _systemUserId = "2fd28110-93d0-427d-9207-d55dbca680fa";

        public DbSet<Item> Items { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<CategoryDetail> CategoriesDetails { get; set; }
        public DbSet<Genre> Genres { get; set; }
        public DbSet<GetItemsForListingDto> ItemsForListing { get; set; }
        public DbSet<AllItemsPipeDelimitedStringDto> AllItemsOutput { get; set; }
        public InventoryDbContext(DbContextOptions<InventoryDbContext> options) : base(options)
        {
        }

        public InventoryDbContext()
        {
        }

        public override int SaveChanges()
        {
            var tracker = ChangeTracker;

            foreach(var entry in tracker.Entries())
            {
                if(entry.Entity is FullAuditModel auditmodel)
                {
                    switch (entry.State)
                    {
                        case EntityState.Added:
                            auditmodel.CreatedDate = DateTime.Now;
                            if(String.IsNullOrWhiteSpace(auditmodel.CreateByUserId))
                               auditmodel.CreateByUserId = _systemUserId;
                            break;

                        case EntityState.Modified:
                        case EntityState.Deleted:
                            auditmodel.LastModifiedDate = DateTime.Now;
                            if(String.IsNullOrWhiteSpace(auditmodel.LastModifiedUserId))
                               auditmodel.LastModifiedUserId = _systemUserId;
                            break;

                        default:
                            break;
                    }
                }
            }

            return base.SaveChanges();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                var builder = new ConfigurationBuilder()
                   .SetBasePath(Directory.GetCurrentDirectory())
                   .AddJsonFile("appsettings.json", reloadOnChange: true, optional: true);

                _configuration = builder.Build();

                var connectionString = _configuration.GetConnectionString("InventoryDatabase");

                optionsBuilder.UseSqlServer(connectionString, settings =>
                {
                    settings.EnableRetryOnFailure();
                });
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Item>()
                .HasMany(e => e.Players)
                .WithMany(e => e.Items)
                .UsingEntity<Dictionary<string, object>>(
                    "ItemPlayers",

                    ip => ip.HasOne<Player>()
                            .WithMany()
                            .HasForeignKey("PlayerId")
                            .OnDelete(DeleteBehavior.Cascade),

                    ip => ip.HasOne<Item>()
                            .WithMany()
                            .HasForeignKey("ItemId")
                            .OnDelete(DeleteBehavior.Cascade)
                );

            modelBuilder
                .Entity<GetItemsForListingDto>()
                .HasNoKey()
                .ToView("ItemsForListing");

            modelBuilder
                .Entity<AllItemsPipeDelimitedStringDto>()
                .HasNoKey()
                .ToView("AllItemsOutput");

            base.OnModelCreating(modelBuilder);
        }
    }
}