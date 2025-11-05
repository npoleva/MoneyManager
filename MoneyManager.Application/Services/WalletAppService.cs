using MoneyManager.Application.Dto;
using MoneyManager.Domain.Aggregates;
using MoneyManager.Domain.Entities;
using MoneyManager.Domain.RepositoryInterfaces;
using MoneyManager.Domain.ValueObjects;

namespace MoneyManager.Application.Services;

public class TransactionsByTypeDto
{
    public string Type { get; set; } = null!;
    public List<TransactionDto> Transactions { get; set; } = new();
    public decimal TotalAmount { get; set; }
}

public class WalletAppService
{
    private readonly IWalletRepository _repository;

    public WalletAppService(IWalletRepository repository)
    {
        _repository = repository;
    }

    public WalletDto CreateWallet(string name, decimal initialBalance, string currency)
    {
        var wallet = new Wallet(name, new Money(initialBalance, currency));
        _repository.Save(wallet);

        return new WalletDto
        {
            Id = wallet.Id,
            Name = wallet.Name,
            InitialBalance = wallet.InitialBalance.Amount,
            Currency = wallet.InitialBalance.Currency
        };
    }

    public void AddTransaction(Guid walletId, TransactionDto dto)
    {
        var wallet = _repository.GetById(walletId);

        var transaction = new Transaction(
            new Money(dto.Amount, dto.Currency),
            dto.Type == "Income" ? TransactionType.Income : TransactionType.Expense,
            new TransactionDescription(dto.Description),
            DateTime.UtcNow
        );

        wallet.AddTransaction(transaction); 
        _repository.AddTransaction(transaction); 
    }

    private List<Transaction> GetTransactionsByMonthAndYearAscending(Guid walletId, int month, int year)
    {
        var wallet = _repository.GetById(walletId);
        return wallet.GetTransactionsByMonthAndType(month, year, TransactionType.Income)
            .Concat(wallet.GetTransactionsByMonthAndType(month, year, TransactionType.Expense))
            .OrderBy(t => t.Date)
            .ToList();
    }

    public List<Transaction> GetTopExpensesByMonthAndYearDescending(Guid walletId, int month, int year, int topCount = 3)
    {
        var wallet = _repository.GetById(walletId);
        return wallet.GetTopExpenses(month, year, topCount);
    }

    public List<TransactionsByTypeDto> GetTransactionsGroupedByTypeDesc(Guid walletId, int month, int year)
    {
        var wallet = _repository.GetById(walletId);
        var grouped = wallet.GetTransactionsGroupedByType(month, year);

        return grouped.Select(g => new TransactionsByTypeDto
        {
            Type = g.Key.ToString(),
            Transactions = g.Value.Select(t => new TransactionDto(
                t.Type.ToString(),
                t.Amount.Amount,
                t.Amount.Currency,
                t.Description.Value,
                t.Date)).ToList(),
            TotalAmount = g.Value.Sum(t => t.Amount.Amount)
        })
        .OrderByDescending(g => g.TotalAmount)
        .ToList();
    }

    public List<Wallet> GetAllWallets() => _repository.GetAll();
}
