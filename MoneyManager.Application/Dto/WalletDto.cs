namespace MoneyManager.Application.Dto;

public class WalletDto
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public decimal InitialBalance { get; set; }
    public string Currency { get; set; }
}