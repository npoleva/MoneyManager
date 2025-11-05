using Microsoft.EntityFrameworkCore;
using MoneyManager.Domain.Aggregates;
using MoneyManager.Domain.Entities;
using MoneyManager.Domain.RepositoryInterfaces;
using MoneyManager.Infrastructure.Data;

namespace MoneyManager.Infrastructure.Repositories;

public class WalletRepository : IWalletRepository
{
    private readonly AppDbContext _dbContext;

    public WalletRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public Wallet GetById(Guid id) =>
        _dbContext.Wallets.Include(w => w.Transactions).FirstOrDefault(w => w.Id == id)!;

    public List<Wallet> GetAll() =>
        _dbContext.Wallets.Include(w => w.Transactions).ToList();

    public void UpdateWallet(Wallet wallet)
    {
        _dbContext.Wallets.Update(wallet);
        _dbContext.SaveChanges();
    }
    
    public void AddTransaction(Transaction transaction)
    {
        _dbContext.Transactions.Add(transaction);
        _dbContext.SaveChanges();
    }
    
    public void Save(Wallet wallet)
    {
        if (!_dbContext.Wallets.Any(w => w.Id == wallet.Id))
            _dbContext.Wallets.Add(wallet);

        _dbContext.SaveChanges();
    }


}