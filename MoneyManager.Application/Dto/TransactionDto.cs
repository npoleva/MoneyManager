namespace MoneyManager.Application.Dto;

public class TransactionDto
{
    public decimal Amount { get; set; }
    public string Currency { get; set; }
    public string Type { get; set; }

    public string Description { get; set; }
    public DateTime Date { get; set; }
    
    public TransactionDto(string type, decimal amount, string currency, string description, DateTime date)
    {
        Type = type;
        Amount = amount;
        Currency = currency;
        Description = description;
        Date = date;
    }
}