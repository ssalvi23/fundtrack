using FundTrack.Domain.Common;

namespace FundTrack.Domain.ValueObjects;

/// <summary>A non-negative rupee amount, always held to 2 decimal places.</summary>
public readonly record struct Money : IComparable<Money>
{
    public static readonly Money Zero = new(0m);

    public decimal Amount { get; }

    public Money(decimal amount)
    {
        if (amount < 0)
        {
            throw new DomainException("Money cannot be negative.");
        }

        Amount = decimal.Round(amount, 2, MidpointRounding.AwayFromZero);
    }

    public static Money operator +(Money left, Money right) => new(left.Amount + right.Amount);

    public static Money operator -(Money left, Money right) => new(left.Amount - right.Amount);

    public static bool operator <(Money left, Money right) => left.Amount < right.Amount;

    public static bool operator >(Money left, Money right) => left.Amount > right.Amount;

    public static bool operator <=(Money left, Money right) => left.Amount <= right.Amount;

    public static bool operator >=(Money left, Money right) => left.Amount >= right.Amount;

    public int CompareTo(Money other) => Amount.CompareTo(other.Amount);

    public override string ToString() => $"INR {Amount:0.00}";
}
