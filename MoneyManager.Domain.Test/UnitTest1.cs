using MoneyManager.Domain.Aggregates;
using MoneyManager.Domain.Entities;
using MoneyManager.Domain.ValueObjects;


namespace MoneyManager.Domain.Test;

public class Tests
{
        [Fact]
        public void AddTransaction_ShouldIncreaseCurrentBalance_WhenIncomeAdded()
        {
            var wallet = new Wallet("Test Wallet", new Money(100, "USD"));
            var transaction = new Transaction(new Money(50, "USD"), TransactionType.Income, new TransactionDescription("Salary"), DateTime.UtcNow);
            
            wallet.AddTransaction(transaction);
            
            Assert.Equal(150, wallet.CurrentBalance.Amount);
        }

        [Fact]
        public void AddTransaction_ShouldDecreaseCurrentBalance_WhenExpenseAdded()
        {
            var wallet = new Wallet("Test Wallet", new Money(100, "USD"));
            var transaction = new Transaction(new Money(30, "USD"), TransactionType.Expense, new TransactionDescription("Groceries"), DateTime.UtcNow);
            
            wallet.AddTransaction(transaction);
            
            Assert.Equal(70, wallet.CurrentBalance.Amount);
        }

        [Fact]
        public void AddTransaction_ShouldThrow_WhenExpenseExceedsBalance()
        {
            var wallet = new Wallet("Test Wallet", new Money(100, "USD"));
            var transaction = new Transaction(new Money(200, "USD"), TransactionType.Expense, new TransactionDescription("Big Purchase"), DateTime.UtcNow);
            
            Assert.Throws<InvalidOperationException>(() => wallet.AddTransaction(transaction));
        }

        [Fact]
        public void GetTransactionsByMonthAndType_ShouldReturnCorrectTransactions()
        {
            var wallet = new Wallet("Test Wallet", new Money(100, "USD"));
            var now = DateTime.UtcNow;
            var income = new Transaction(new Money(50, "USD"), TransactionType.Income, new TransactionDescription("Salary"), now);
            var expense = new Transaction(new Money(20, "USD"), TransactionType.Expense, new TransactionDescription("Groceries"), now);

            wallet.AddTransaction(income);
            wallet.AddTransaction(expense);
            
            var incomes = wallet.GetTransactionsByMonthAndType(now.Month, now.Year, TransactionType.Income);
            var expenses = wallet.GetTransactionsByMonthAndType(now.Month, now.Year, TransactionType.Expense);
            
            Assert.Single(incomes);
            Assert.Single(expenses);
            Assert.Equal(income.Id, incomes.First().Id);
            Assert.Equal(expense.Id, expenses.First().Id);
        }

        [Fact]
        public void GetTopExpenses_ShouldReturnTopNExpenses()
        {
            var wallet = new Wallet("Test Wallet", new Money(1000, "USD"));
            var now = DateTime.UtcNow;

            wallet.AddTransaction(new Transaction(new Money(50, "USD"), TransactionType.Expense, new TransactionDescription("A"), now));
            wallet.AddTransaction(new Transaction(new Money(100, "USD"), TransactionType.Expense, new TransactionDescription("B"), now));
            wallet.AddTransaction(new Transaction(new Money(75, "USD"), TransactionType.Expense, new TransactionDescription("C"), now));
            
            var top2 = wallet.GetTopExpenses(now.Month, now.Year, 2);
            
            Assert.Equal(2, top2.Count);
            Assert.Equal(100, top2[0].Amount.Amount); 
            Assert.Equal(75, top2[1].Amount.Amount);
        }

        [Fact]
        public void GetTransactionsGroupedByType_ShouldReturnDictionaryGroupedByType()
        {
            var wallet = new Wallet("Test Wallet", new Money(500, "USD"));
            var now = DateTime.UtcNow;

            var income = new Transaction(new Money(100, "USD"), TransactionType.Income, new TransactionDescription("Salary"), now);
            var expense = new Transaction(new Money(50, "USD"), TransactionType.Expense, new TransactionDescription("Groceries"), now);

            wallet.AddTransaction(income);
            wallet.AddTransaction(expense);
            
            var grouped = wallet.GetTransactionsGroupedByType(now.Month, now.Year);
            
            Assert.Equal(2, grouped.Count);
            Assert.True(grouped.ContainsKey(TransactionType.Income));
            Assert.True(grouped.ContainsKey(TransactionType.Expense));
            Assert.Single(grouped[TransactionType.Income]);
            Assert.Single(grouped[TransactionType.Expense]);
        }
}