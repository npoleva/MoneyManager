using System.Globalization;
using MoneyManager.Application.Dto;
using MoneyManager.Application.Services;

namespace MoneyManager.ConsoleApplication;

public class ConsoleApplication
{
    private readonly WalletAppService _walletService;

    public ConsoleApplication(WalletAppService walletService)
    {
        _walletService = walletService;
    }

    public void Run()
    {
        Console.WriteLine("Добро пожаловать в Money Manager!");

        while (true)
        {
            ShowMenu();
            var choice = Console.ReadLine();

            switch (choice)
            {
                case "1": CreateWallet(); break;
                case "2": AddTransaction(); break;
                case "3": ShowWallets(); break;
                case "4": GetTransactionGroupsByTypeSortedByTotalAmountForMonth(); break;
                case "5": ShowTop3Expenses(); break;
                case "6": ShowWallets(); break;
                case "0": return;
                
                default: Console.WriteLine("Неверный выбор."); break;
            }
        }
    }
    private void ShowMenu()
    {
        Console.WriteLine("\nВыберите действие:");
        Console.WriteLine("1 - Создать кошелек");
        Console.WriteLine("2 - Добавить транзакцию");
        Console.WriteLine("3 - Показать баланс и транзакции");
        Console.WriteLine("4 - Показать сводку транзакций за месяц");
        Console.WriteLine("5 - Топ-3 расходов за месяц для каждого кошелька");
        Console.WriteLine("6 - Показать все кошельки");
        Console.WriteLine("0 - Выход");
    }

    private void CreateWallet()
    {
        Console.Write("Название кошелька: ");
        var name = Console.ReadLine()!;
        Console.Write("Валюта: ");
        var currency = Console.ReadLine()!;
        Console.Write("Начальный баланс: ");
        var balance = decimal.Parse(Console.ReadLine()!, CultureInfo.InvariantCulture);

        var wallet = _walletService.CreateWallet(name, balance, currency);

        Console.WriteLine("\nКошелек создан!");
        Console.WriteLine($"ID: {wallet.Id}");
        Console.WriteLine($"Название: {wallet.Name}");
        Console.WriteLine($"Валюта: {wallet.Currency}");
        Console.WriteLine($"Начальный баланс: {wallet.InitialBalance}");
    }

    private void ShowWallets()
    {
        var wallets = _walletService.GetAllWallets();

        if (wallets.Count == 0)
        {
            Console.WriteLine("Кошельки отсутствуют.");
            return;
        }

        Console.WriteLine("\nСписок кошельков:");
        for (int i = 0; i < wallets.Count; i++)
        {
            var w = wallets[i];
            Console.WriteLine($"{i + 1} - {w.Name} ({w.InitialBalance.Currency}) | Начальный: {w.InitialBalance.Amount}, Текущий: {w.CurrentBalance.Amount}");
        }
    }

    private void AddTransaction()
    {
        var wallets = _walletService.GetAllWallets();
        if (wallets.Count == 0)
        {
            Console.WriteLine("Сначала создайте кошелек!");
            return;
        }

        ShowWallets();

        int walletChoice;
        while (true)
        {
            Console.Write("Выберите номер кошелька: ");
            if (int.TryParse(Console.ReadLine(), out walletChoice) &&
                walletChoice >= 1 && walletChoice <= wallets.Count)
                break;
            Console.WriteLine("Неверный номер. Попробуйте снова.");
        }

        var selectedWallet = wallets[walletChoice - 1];

        decimal amount;
        while (true)
        {
            Console.Write("Сумма: ");
            if (decimal.TryParse(Console.ReadLine(), out amount))
                break;
            Console.WriteLine("Неверная сумма. Попробуйте снова.");
        }

        string type;
        while (true)
        {
            Console.Write("Тип (Income/Expense): ");
            type = Console.ReadLine()!;
            if (type == "Income" || type == "Expense")
                break;
            Console.WriteLine("Введите либо 'Income', либо 'Expense'.");
        }

        Console.Write("Описание: ");
        var description = Console.ReadLine()!;

        var dto = new TransactionDto(type, amount, selectedWallet.InitialBalance.Currency, description, DateTime.UtcNow);

        try
        {
            _walletService.AddTransaction(selectedWallet.Id, dto);
            Console.WriteLine("Транзакция добавлена!");
        }
        catch (InvalidOperationException ex)
        {
            Console.WriteLine($"Ошибка: {ex.Message}");
        }
    }

    private void GetTransactionGroupsByTypeSortedByTotalAmountForMonth()
    {
        var wallets = _walletService.GetAllWallets();
        if (wallets.Count == 0)
        {
            Console.WriteLine("Кошельки отсутствуют.");
            return;
        }

        int walletChoice;
        ShowWallets();
        while (true)
        {
            Console.Write("Выберите номер кошелька для просмотра транзакций: ");
            if (int.TryParse(Console.ReadLine(), out walletChoice) &&
                walletChoice >= 1 && walletChoice <= wallets.Count)
                break;
            Console.WriteLine("Неверный номер. Попробуйте снова.");
        }
        var selectedWallet = wallets[walletChoice - 1];

        int month = ReadMonth();
        int year = ReadYear();

        var grouped = _walletService.GetTransactionsGroupedByTypeDesc(selectedWallet.Id, month, year);

        foreach (var group in grouped)
        {
            Console.WriteLine($"\n--- {group.Type} | Сумма: {group.TotalAmount} ---");
            foreach (var t in group.Transactions)
            {
                Console.WriteLine($"{t.Date:yyyy-MM-dd} {t.Type} {t.Amount} {t.Currency} - {t.Description}");
            }
        }
    }

    private void ShowTop3Expenses()
    {
        var wallets = _walletService.GetAllWallets();
        if (wallets.Count == 0)
        {
            Console.WriteLine("Кошельки отсутствуют.");
            return;
        }

        int month = ReadMonth();
        int year = ReadYear();

        foreach (var wallet in wallets)
        {
            var topExpenses = _walletService.GetTopExpensesByMonthAndYearDescending(wallet.Id, month, year);

            Console.WriteLine($"\nКошелек: {wallet.Name} ({wallet.InitialBalance.Currency})");
            Console.WriteLine($"Топ-{topExpenses.Count} расходов за {month}/{year}:");
            foreach (var t in topExpenses)
            {
                Console.WriteLine($"{t.Date:yyyy-MM-dd} {t.Type} {t.Amount.Amount} {t.Amount.Currency} - {t.Description.Value}");
            }
        }
    }

    private int ReadMonth()
    {
        int month;
        while (true)
        {
            Console.Write("Месяц (1-12): ");
            if (int.TryParse(Console.ReadLine(), out month) && month >= 1 && month <= 12)
                break;
            Console.WriteLine("Неверный месяц. Попробуйте снова.");
        }
        return month;
    }

    private int ReadYear()
    {
        int year;
        while (true)
        {
            Console.Write("Год: ");
            if (int.TryParse(Console.ReadLine(), out year) && year > 2000 && year <= 2100)
                break;
            Console.WriteLine("Неверный год. Попробуйте снова.");
        }
        return year;
    }
}