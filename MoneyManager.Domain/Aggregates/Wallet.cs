using MoneyManager.Domain.Entities;
using MoneyManager.Domain.ValueObjects;

namespace MoneyManager.Domain.Aggregates;

public class Wallet
    {
        public Guid Id { get; private set; }
        public string Name { get; private set; } = null!;
        public Money InitialBalance { get; private set; } = null!;

        private readonly List<Transaction> _transactions = new();
        
        protected Wallet() { }

        public Wallet(string name, Money initialBalance)
        {
            Id = Guid.NewGuid();
            Name = name;
            InitialBalance = initialBalance;
        }
        public IReadOnlyList<Transaction> Transactions => _transactions.AsReadOnly();

        public Money CurrentBalance
        {
            get
            {
                var income = _transactions
                    .Where(t => t.Type == TransactionType.Income)
                    .Select(t => t.Amount)
                    .Aggregate(new Money(0, InitialBalance.Currency), (a, b) => a + b);

                var expense = _transactions
                    .Where(t => t.Type == TransactionType.Expense)
                    .Select(t => t.Amount)
                    .Aggregate(new Money(0, InitialBalance.Currency), (a, b) => a + b);

                return InitialBalance + income - expense;
            }
        }

        public void AddTransaction(Transaction transaction)
        {
            if (transaction.Type == TransactionType.Expense && !CurrentBalance.IsGreaterThan(transaction.Amount))
                throw new InvalidOperationException("Недостаточно средств для этой траты.");

            _transactions.Add(transaction);
        }

        public List<Transaction> GetTransactionsByMonthAndType(int month, int year, TransactionType type)
        {
            return _transactions
                .Where(t => t.Type == type && t.Date.Month == month && t.Date.Year == year)
                .OrderBy(t => t.Date)
                .ToList();
        }

        public List<Transaction> GetTopExpenses(int month, int year, int topCount = 3)
        {
            return _transactions
                .Where(t => t.Type == TransactionType.Expense && t.Date.Month == month && t.Date.Year == year)
                .OrderByDescending(t => t.Amount.Amount)
                .Take(topCount)
                .ToList();
        }

        public Dictionary<TransactionType, List<Transaction>> GetTransactionsGroupedByType(int month, int year)
        {
            return _transactions
                .Where(t => t.Date.Month == month && t.Date.Year == year)
                .GroupBy(t => t.Type)
                .ToDictionary(g => g.Key, g => g.ToList());
        }
    }