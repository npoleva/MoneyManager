using MoneyManager.Domain.ValueObjects;

namespace MoneyManager.Domain.Entities;

public enum TransactionType { Income, Expense }

public class Transaction
{
    public Guid Id { get; }
    public DateTime Date { get; }
    public Money Amount { get; }
    public TransactionType Type { get; }
    public TransactionDescription Description { get; }

    public Transaction(Money amount, TransactionType type, TransactionDescription description, DateTime date)
    {
        Id = Guid.NewGuid();
        Amount = amount;
        Type = type;
        Description = description;
        Date = date;
    }
    private Transaction() { }
}