using Microsoft.EntityFrameworkCore;
using MoneyManager.Application.Services;
using MoneyManager.Infrastructure.Data;
using MoneyManager.Infrastructure.Repositories;

namespace MoneyManager.ConsoleApplication;

public static class Program
{
    static void Main()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseNpgsql("Host=localhost;Port=5432;Database=money_manager_db;Username=postgres;Password=postgres")
            .Options;

        var dbContext = new AppDbContext(options);
        var walletService = new WalletAppService(new WalletRepository(dbContext));

        var app = new ConsoleApplication(walletService);
        app.Run();
    }
}