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
        private const string SYSTEM_USER_ID = "2fd28110-93d0-427d-9207-d55dbca680fa";

        public DbSet<Item> Items { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<CategoryDetail> CategoryDetails { get; set; }
        public DbSet<Genre> Genres { get; set; }
        public DbSet<GetItemsForListingDto> ItemsForListing { get; set; }
        public DbSet<AllItemsPipeDelimitedStringDto> AllItemsOutput { get; set; }
        public DbSet<GetItemsTotalValueDto> GetItemsTotalValues { get; set; }
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
                if(entry.Entity is FullAuditModel auditModel)
                {
                    switch (entry.State)
                    {
                        case EntityState.Added:
                            auditModel.CreatedDate = DateTime.Now;
                            if(string.IsNullOrWhiteSpace(auditModel.CreatedByUserId))
                               auditModel.CreatedByUserId = SYSTEM_USER_ID;
                            break;

                        case EntityState.Modified:
                        case EntityState.Deleted:
                            auditModel.LastModifiedDate = DateTime.Now;
                            if(string.IsNullOrWhiteSpace(auditModel.LastModifiedUserId))
                               auditModel.LastModifiedUserId = SYSTEM_USER_ID;
                            break;

                        case EntityState.Detached:
                        case EntityState.Unchanged:
                        default:
                            break;
                    }
                }
            }

            return base.SaveChanges();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (optionsBuilder.IsConfigured) return;

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
                            .OnDelete(DeleteBehavior.ClientCascade)
                );

            modelBuilder
                .Entity<GetItemsForListingDto>()
                .HasNoKey()
                .ToView("ItemsForListing");

            modelBuilder
                .Entity<AllItemsPipeDelimitedStringDto>()
                .HasNoKey()
                .ToView("AllItemsOutput");

            modelBuilder.Entity<GetItemsTotalValueDto>()
                .HasNoKey()
                .ToView("GetItemsTotalValues");

            var genreCreateDate = new DateTime(2021, 01, 01);
            modelBuilder.Entity<Genre>(g =>
            {
                g.HasData(
                    new Genre { Id = 1, CreatedDate = genreCreateDate, IsActive = true, IsDeleted = false, Name = "Fantasy" },
                    new Genre { Id = 2, CreatedDate = genreCreateDate, IsActive = true, IsDeleted = false, Name = "Sci/Fi" },
                    new Genre { Id = 3, CreatedDate = genreCreateDate, IsActive = true, IsDeleted = false, Name = "Horror" },
                    new Genre { Id = 4, CreatedDate = genreCreateDate, IsActive = true, IsDeleted = false, Name = "Comedy" },
                    new Genre { Id = 5, CreatedDate = genreCreateDate, IsActive = true, IsDeleted = false, Name = "Drama" }
                );
            });

            base.OnModelCreating(modelBuilder);
        }
    }
}