using ImageEditor.Business.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ImageEditor.Data.Mappings
{
    public class UserMap : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("TB_User");

            builder.HasKey(e => e.Id);

            builder
                .Property(e => e.Name)
                .IsRequired()
                .HasColumnType("varchar(250)");

            builder
                .Property(e => e.Email)
                .IsRequired()
                .HasColumnType("varchar(250)");

            builder
                .HasMany(e => e.Images)
                .WithOne(e => e.User);
        }
    }
}