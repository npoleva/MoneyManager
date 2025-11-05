using MoneyManager.Domain.Aggregates;
using MoneyManager.Domain.Entities;

namespace MoneyManager.Domain.RepositoryInterfaces;


public interface IWalletRepository
{
    Wallet GetById(Guid id);
    List<Wallet> GetAll();
    void Save(Wallet wallet);
    void AddTransaction(Transaction transaction);
}