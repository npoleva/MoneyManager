namespace MoneyManager.Domain.ValueObjects;

public record TransactionDescription
{
    public string Value { get; }

    public TransactionDescription(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("Описание не может быть пустым");
        Value = value;
    }

    public override string ToString() => Value;
}