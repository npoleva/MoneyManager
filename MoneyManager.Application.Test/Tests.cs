using Moq;
using MoneyManager.Application.Dto;
using MoneyManager.Application.Services;
using MoneyManager.Domain.Aggregates;
using MoneyManager.Domain.Entities;
using MoneyManager.Domain.RepositoryInterfaces;
using MoneyManager.Domain.ValueObjects;

namespace MoneyManager.Application.Test;

public class Tests
{
    private readonly Mock<IWalletRepository> _repositoryMock;
        private readonly WalletAppService _service;
        private readonly Wallet _wallet;

        public Tests()
        {
            _repositoryMock = new Mock<IWalletRepository>();
            _service = new WalletAppService(_repositoryMock.Object);

            _wallet = new Wallet("TestWallet", new Money(100, "USD"));
            _repositoryMock.Setup(r => r.GetById(It.IsAny<Guid>())).Returns(_wallet);
            _repositoryMock.Setup(r => r.GetAll()).Returns(new List<Wallet> { _wallet });
        }

        [Fact]
        public void CreateWallet_ShouldReturnWalletDtoAndSaveWallet()
        {
            var name = "MyWallet";
            var initialBalance = 200m;
            var currency = "USD";
            
            var result = _service.CreateWallet(name, initialBalance, currency);
            
            Assert.NotNull(result);
            Assert.Equal(name, result.Name);
            Assert.Equal(initialBalance, result.InitialBalance);
            Assert.Equal(currency, result.Currency);
            _repositoryMock.Verify(r => r.Save(It.IsAny<Wallet>()), Times.Once);
        }

        [Fact]
        public void AddTransaction_ShouldAddTransactionToWalletAndCallRepository()
        {
            var dto = new TransactionDto("Income", 50, "USD", "Salary", DateTime.UtcNow);
            
            _service.AddTransaction(_wallet.Id, dto);
            
            Assert.Single(_wallet.Transactions);
            var transaction = _wallet.Transactions.First();
            Assert.Equal(dto.Amount, transaction.Amount.Amount);
            Assert.Equal(dto.Currency, transaction.Amount.Currency);
            Assert.Equal(dto.Type, transaction.Type.ToString());
            Assert.Equal(dto.Description, transaction.Description.Value);

            _repositoryMock.Verify(r => r.AddTransaction(It.IsAny<Transaction>()), Times.Once);
        }

        [Fact]
        public void GetTopExpensesByMonthAndYearDescending_ShouldReturnTopExpenses()
        {
            _wallet.AddTransaction(new Transaction(new Money(10, "USD"), TransactionType.Expense, new TransactionDescription("A"), new DateTime(2025, 11, 1)));
            _wallet.AddTransaction(new Transaction(new Money(50, "USD"), TransactionType.Expense, new TransactionDescription("B"), new DateTime(2025, 11, 2)));
            _wallet.AddTransaction(new Transaction(new Money(20, "USD"), TransactionType.Expense, new TransactionDescription("C"), new DateTime(2025, 11, 3)));
            
            var topExpenses = _service.GetTopExpensesByMonthAndYearDescending(_wallet.Id, 11, 2025, 2);
            
            Assert.Equal(2, topExpenses.Count);
            Assert.Equal(50, topExpenses[0].Amount.Amount);
            Assert.Equal(20, topExpenses[1].Amount.Amount);
        }

        [Fact]
        public void GetTransactionsGroupedByTypeDesc_ShouldReturnGroupedTransactions()
        {
            _wallet.AddTransaction(new Transaction(new Money(10, "USD"), TransactionType.Expense, new TransactionDescription("Expense1"), new DateTime(2025, 11, 1)));
            _wallet.AddTransaction(new Transaction(new Money(20, "USD"), TransactionType.Income, new TransactionDescription("Income1"), new DateTime(2025, 11, 2)));
            
            var grouped = _service.GetTransactionsGroupedByTypeDesc(_wallet.Id, 11, 2025);
            
            Assert.Equal(2, grouped.Count);

            var expenseGroup = grouped.First(g => g.Type == "Expense");
            Assert.Single(expenseGroup.Transactions);
            Assert.Equal(10, expenseGroup.TotalAmount);

            var incomeGroup = grouped.First(g => g.Type == "Income");
            Assert.Single(incomeGroup.Transactions);
            Assert.Equal(20, incomeGroup.TotalAmount);
        }

        [Fact]
        public void GetAllWallets_ShouldReturnWalletsFromRepository()
        {
            var wallets = _service.GetAllWallets();
            
            Assert.Single(wallets);
            Assert.Equal("TestWallet", wallets[0].Name);
        }

        [Fact]
        public void AddTransaction_ExpenseMoreThanBalance_ShouldThrowException()
        {
            var dto = new TransactionDto("Expense", 200, "USD", "BigExpense", DateTime.UtcNow);
            
            var ex = Assert.Throws<InvalidOperationException>(() => _service.AddTransaction(_wallet.Id, dto));
            Assert.Equal("Недостаточно средств для этой траты.", ex.Message);
        }
}