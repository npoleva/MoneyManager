using Microsoft.EntityFrameworkCore;
using MoneyManager.Domain.Aggregates; 
using MoneyManager.Domain.Entities;
using MoneyManager.Infrastructure.Data.Configurations; 

namespace MoneyManager.Infrastructure.Data;

public class AppDbContext : DbContext
{
    public DbSet<Wallet> Wallets { get; set; } = null!;
    public DbSet<Transaction> Transactions { get; set; } = null!;

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfiguration(new WalletConfiguration());
        modelBuilder.ApplyConfiguration(new TransactionConfiguration());
    }
}
