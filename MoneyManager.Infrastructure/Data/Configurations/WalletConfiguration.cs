using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MoneyManager.Domain.Aggregates;

namespace MoneyManager.Infrastructure.Data.Configurations;

public class WalletConfiguration : IEntityTypeConfiguration<Wallet>
{
    public void Configure(EntityTypeBuilder<Wallet> builder)
    {
        builder.HasKey(w => w.Id);
        builder.Property(w => w.Name).IsRequired().HasMaxLength(100);

        builder.OwnsOne(w => w.InitialBalance, mb =>
        {
            mb.Property(m => m.Amount).HasColumnName("InitialBalance");
            mb.Property(m => m.Currency).HasColumnName("Currency");
        });

        builder.HasMany(w => w.Transactions)
            .WithOne()
            .HasForeignKey("WalletId")
            .OnDelete(DeleteBehavior.Cascade);
    }
}
