using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ImageEditor.Data.Contexts;
public class ImageEditorIdentityDbContext : IdentityDbContext
{
    public ImageEditorIdentityDbContext(DbContextOptions<ImageEditorIdentityDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema(ImageEditorContext.SCHEMA);
        base.OnModelCreating(modelBuilder);
    }
}