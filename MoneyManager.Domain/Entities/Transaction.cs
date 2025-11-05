using MoneyManager.Domain.ValueObjects;

namespace MoneyManager.Domain.Entities;

public enum TransactionType { Income, Expense }

public class Transaction
{
    public Guid Id { get; private set; } 
    public Money Amount { get; private set; }
    public TransactionType Type { get; private set; }
    public TransactionDescription Description { get; private set; }
    public DateTime Date { get; private set; }
    
    protected Transaction() { }

    public Transaction(Money amount, TransactionType type, TransactionDescription description, DateTime date)
    {
        Id = Guid.NewGuid();
        Amount = amount;
        Type = type;
        Description = description;
        Date = date;
    }
}