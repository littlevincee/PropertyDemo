using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace PropertyDemo.Data.Configuration
{
    public class OwnerDetailConfiguration : IEntityTypeConfiguration<Model.OwnerDetail>
    {
        public void Configure(EntityTypeBuilder<Model.OwnerDetail> builder)
        {
            // PK
            builder.HasKey(t => t.OwnerDetailId);
            builder.Property(x => x.OwnerDetailId)
                    .ValueGeneratedOnAdd();

            // Table & Column Mappings
            builder.ToTable("OwnerDetail");

            builder.Property(x => x.FirstName)
                    .HasColumnType("nvarchar")
                    .HasMaxLength(50)
                    .IsRequired();

            builder.Property(x => x.Surname)
                    .HasColumnType("nvarchar")
                    .HasMaxLength(50)
                    .IsRequired();

            builder.Property(x => x.HongKongId)
                    .HasColumnType("varchar")
                    .HasMaxLength(8)
                    .IsRequired();

            builder.Property(x => x.ContactNumber)
                    .IsRequired();

            builder.Property(x => x.CreatedOn)
                    .IsRequired();

            // Index
            builder.HasIndex(x => new { x.FirstName, x.Surname, x.HongKongId })
                    .IsUnique();

            builder.HasIndex(x => x.HongKongId)
                    .IsUnique();
        }
    }
}
