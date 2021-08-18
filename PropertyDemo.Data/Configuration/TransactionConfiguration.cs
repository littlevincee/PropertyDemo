using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace PropertyDemo.Data.Configuration
{
    public class TransactionConfiguration : IEntityTypeConfiguration<Model.Transaction>
    {
        public void Configure(EntityTypeBuilder<Model.Transaction> builder)
        {
            // PK
            builder.HasKey(t => t.TransactionId);
            builder.Property(x => x.TransactionId)
                    .ValueGeneratedOnAdd();

            // Table & Column Mappings
            builder.ToTable("Transaction");

            builder.Property(x => x.TransactionDate)
                    .IsRequired();

            builder.Property(x => x.TransactionAmount)
                    .HasPrecision(15, 2)
                    .IsRequired();

            builder.Property(x => x.PaymentMethod)
                    .HasColumnType("varchar")
                    .HasMaxLength(50)
                    .IsRequired();

            builder.Property(x => x.BankName)
                    .HasColumnType("varchar")
                    .HasMaxLength(50)
                    .IsRequired();

            builder.Property(x => x.TransactionType)
                    .HasColumnType("varchar")
                    .HasMaxLength(10)
                    .IsRequired();

            builder.Property(x => x.IsDeposit)
                    .IsRequired();

            builder.Property(x => x.CreatedOn)
                    .IsRequired();

            // Relationships
            builder.HasOne(t => t.Property)
                    .WithMany(t => t.Transactions)
                    .HasForeignKey(t => t.PropertyId)
                    .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(t => t.OwnerDetail)
                    .WithMany(t => t.Transactions)
                    .HasForeignKey(t => t.OwnerDetailId)
                    .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(t => t.ApplicationUser)
                .WithMany(t => t.Transactions)
                .HasForeignKey(t => t.ApplicationUserId)
                .OnDelete(DeleteBehavior.NoAction)
                .IsRequired();



        }
    }
}
