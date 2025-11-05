namespace MoneyManager.Domain.ValueObjects;

public record Money(decimal Amount, string Currency)
{
    public static Money operator +(Money a, Money b)
    {
        if (a.Currency != b.Currency)
            throw new InvalidOperationException("Разные валюты");
        return new Money(a.Amount + b.Amount, a.Currency);
    }

    public static Money operator -(Money a, Money b)
    {
        if (a.Currency != b.Currency)
            throw new InvalidOperationException("Разные валюты");
        return new Money(a.Amount - b.Amount, a.Currency);
    }

    public bool IsGreaterThan(Money other)
    {
        if (Currency != other.Currency)
            throw new InvalidOperationException("Разные валюты");
        return Amount > other.Amount;
    }
}