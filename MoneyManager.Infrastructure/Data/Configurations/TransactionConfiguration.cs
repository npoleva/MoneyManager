using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MoneyManager.Domain.Entities;

namespace MoneyManager.Infrastructure.Data.Configurations;

public class TransactionConfiguration : IEntityTypeConfiguration<Transaction>
{
    public void Configure(EntityTypeBuilder<Transaction> builder)
    {
        builder.HasKey(t => t.Id);

        builder.Property(t => t.Date).IsRequired();
        builder.Property(t => t.Type).IsRequired();

        builder.OwnsOne(t => t.Amount, mb =>
        {
            mb.Property(m => m.Amount).HasColumnName("Amount");
            mb.Property(m => m.Currency).HasColumnName("Currency");
        });

        builder.OwnsOne(t => t.Description, mb =>
        {
            mb.Property(d => d.Value).HasColumnName("Description");
        });
    }
}
