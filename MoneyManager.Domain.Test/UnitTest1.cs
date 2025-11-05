using MoneyManager.Domain.Aggregates;
using MoneyManager.Domain.Entities;
using MoneyManager.Domain.ValueObjects;


namespace MoneyManager.Domain.Test;

public class Tests
{
        [Fact]
        public void AddTransaction_ShouldIncreaseCurrentBalance_WhenIncomeAdded()
        {
            // Arrange
            var wallet = new Wallet("Test Wallet", new Money(100, "USD"));
            var transaction = new Transaction(new Money(50, "USD"), TransactionType.Income, new TransactionDescription("Salary"), DateTime.UtcNow);

            // Act
            wallet.AddTransaction(transaction);

            // Assert
            Assert.Equal(150, wallet.CurrentBalance.Amount);
        }

        [Fact]
        public void AddTransaction_ShouldDecreaseCurrentBalance_WhenExpenseAdded()
        {
            // Arrange
            var wallet = new Wallet("Test Wallet", new Money(100, "USD"));
            var transaction = new Transaction(new Money(30, "USD"), TransactionType.Expense, new TransactionDescription("Groceries"), DateTime.UtcNow);

            // Act
            wallet.AddTransaction(transaction);

            // Assert
            Assert.Equal(70, wallet.CurrentBalance.Amount);
        }

        [Fact]
        public void AddTransaction_ShouldThrow_WhenExpenseExceedsBalance()
        {
            // Arrange
            var wallet = new Wallet("Test Wallet", new Money(100, "USD"));
            var transaction = new Transaction(new Money(200, "USD"), TransactionType.Expense, new TransactionDescription("Big Purchase"), DateTime.UtcNow);

            // Act & Assert
            Assert.Throws<InvalidOperationException>(() => wallet.AddTransaction(transaction));
        }

        [Fact]
        public void GetTransactionsByMonthAndType_ShouldReturnCorrectTransactions()
        {
            // Arrange
            var wallet = new Wallet("Test Wallet", new Money(100, "USD"));
            var now = DateTime.UtcNow;
            var income = new Transaction(new Money(50, "USD"), TransactionType.Income, new TransactionDescription("Salary"), now);
            var expense = new Transaction(new Money(20, "USD"), TransactionType.Expense, new TransactionDescription("Groceries"), now);

            wallet.AddTransaction(income);
            wallet.AddTransaction(expense);

            // Act
            var incomes = wallet.GetTransactionsByMonthAndType(now.Month, now.Year, TransactionType.Income);
            var expenses = wallet.GetTransactionsByMonthAndType(now.Month, now.Year, TransactionType.Expense);

            // Assert
            Assert.Single(incomes);
            Assert.Single(expenses);
            Assert.Equal(income.Id, incomes.First().Id);
            Assert.Equal(expense.Id, expenses.First().Id);
        }

        [Fact]
        public void GetTopExpenses_ShouldReturnTopNExpenses()
        {
            // Arrange
            var wallet = new Wallet("Test Wallet", new Money(1000, "USD"));
            var now = DateTime.UtcNow;

            wallet.AddTransaction(new Transaction(new Money(50, "USD"), TransactionType.Expense, new TransactionDescription("A"), now));
            wallet.AddTransaction(new Transaction(new Money(100, "USD"), TransactionType.Expense, new TransactionDescription("B"), now));
            wallet.AddTransaction(new Transaction(new Money(75, "USD"), TransactionType.Expense, new TransactionDescription("C"), now));

            // Act
            var top2 = wallet.GetTopExpenses(now.Month, now.Year, 2);

            // Assert
            Assert.Equal(2, top2.Count);
            Assert.Equal(100, top2[0].Amount.Amount); // самый большой первый
            Assert.Equal(75, top2[1].Amount.Amount);
        }

        [Fact]
        public void GetTransactionsGroupedByType_ShouldReturnDictionaryGroupedByType()
        {
            // Arrange
            var wallet = new Wallet("Test Wallet", new Money(500, "USD"));
            var now = DateTime.UtcNow;

            var income = new Transaction(new Money(100, "USD"), TransactionType.Income, new TransactionDescription("Salary"), now);
            var expense = new Transaction(new Money(50, "USD"), TransactionType.Expense, new TransactionDescription("Groceries"), now);

            wallet.AddTransaction(income);
            wallet.AddTransaction(expense);

            // Act
            var grouped = wallet.GetTransactionsGroupedByType(now.Month, now.Year);

            // Assert
            Assert.Equal(2, grouped.Count);
            Assert.True(grouped.ContainsKey(TransactionType.Income));
            Assert.True(grouped.ContainsKey(TransactionType.Expense));
            Assert.Single(grouped[TransactionType.Income]);
            Assert.Single(grouped[TransactionType.Expense]);
        }
}