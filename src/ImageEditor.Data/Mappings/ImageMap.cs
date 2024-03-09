using ImageEditor.Business.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ImageEditor.Data.Mappings;
public class ImageMap : IEntityTypeConfiguration<Image>
{
    public void Configure(EntityTypeBuilder<Image> builder)
    {
        builder.ToTable("TB_Image");

        builder.HasKey(e => e.Id);

        builder
            .Property(e => e.S3EditedImage)
            .IsRequired()
            .HasColumnType("varchar(1024)");

        builder
            .Property(e => e.S3OriginalImage)
            .IsRequired()
            .HasColumnType("varchar(1024)");

        builder
            .Property(e => e.UserId)
            .IsRequired()
            .HasColumnType("uuid");

        builder
            .HasOne(e => e.User)
            .WithMany(e => e.Images)
            .HasForeignKey(e => e.UserId);
    }
}
