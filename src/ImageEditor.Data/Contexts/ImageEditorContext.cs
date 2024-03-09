using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ImageEditor.Business.Models;
using Microsoft.EntityFrameworkCore;

namespace ImageEditor.Data.Contexts;
public class ImageEditorContext : DbContext
{
    public const string SCHEMA = "ImageEditor";

    public ImageEditorContext(DbContextOptions<ImageEditorContext> options) : base(options)
    {
        ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
        ChangeTracker.AutoDetectChangesEnabled = false;
    }

    public DbSet<Image> Images { get; set; }
    public DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema(SCHEMA);

        foreach (var property in modelBuilder.Model.GetEntityTypes()
            .SelectMany(e => e.GetProperties()
                .Where(p => p.ClrType == typeof(string))))
            property.SetColumnType("varchar(100)");

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ImageEditorContext).Assembly);

        foreach (var relationship in modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
            relationship.DeleteBehavior = DeleteBehavior.ClientSetNull;

        base.OnModelCreating(modelBuilder);
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
    {
        foreach (var entry in ChangeTracker.Entries().Where(entry => entry.Entity.GetType().GetProperty("CreatedIn") != null))
        {
            if (entry.State == EntityState.Added)
            {
                entry.Property("CreatedIn").CurrentValue = DateTime.Now;
            }

            if (entry.State == EntityState.Modified)
            {
                entry.Property("CreatedIn").IsModified = false;
            }
        }

        return base.SaveChangesAsync(cancellationToken);
    }
}
