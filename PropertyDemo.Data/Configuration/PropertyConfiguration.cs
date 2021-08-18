using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace PropertyDemo.Data.Configuration
{
    public class PropertyConfiguration : IEntityTypeConfiguration<Model.Property>
    {
        public void Configure(EntityTypeBuilder<Model.Property> builder)
        {
            // PK
            builder.HasKey(t => t.PropertyId);
            builder.Property(x => x.PropertyId)
                    .ValueGeneratedOnAdd();

            // Table & Column Mappings
            builder.ToTable("Property");
            builder.Property(x => x.PropertyName)
                    .HasColumnType("nvarchar")
                    .HasMaxLength(25)
                    .IsRequired();

            builder.Property(x => x.Bedroom)
                    .IsRequired();

            builder.Property(x => x.IsAvaliable)
                    .IsRequired();

            builder.Property(x => x.SalePrice)
                    .HasPrecision(15, 2)
                    .IsRequired();

            builder.Property(x => x.LeasePrice)
                    .HasPrecision(15, 2)
                    .IsRequired();

            builder.Property(x => x.CreatedOn)
                    .IsRequired();

            // Relationships
            builder.HasOne(t => t.ApplicationUser)
                    .WithMany(t => t.Properties)
                    .HasForeignKey(t => t.ApplicationUserId)
                    .OnDelete(DeleteBehavior.NoAction)
                    .IsRequired();

            builder.HasOne(t => t.OwnerDetail)
                    .WithMany(t => t.Properties)
                    .HasForeignKey(t => t.OwnerDetailId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .IsRequired();
        }
    }
}
