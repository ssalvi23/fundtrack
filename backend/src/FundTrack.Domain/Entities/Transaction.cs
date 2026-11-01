using FundTrack.Domain.Common;
using FundTrack.Domain.Enums;
using FundTrack.Domain.ValueObjects;

namespace FundTrack.Domain.Entities;

/// <summary>Immutable ledger row. A mistake is corrected by a new transaction, never by an edit.</summary>
public sealed class Transaction : AuditableEntity
{
    private const int UnitDecimals = 3;

    private Transaction()
    {
    }

    public Guid UserId { get; private set; }
    public Guid SchemeId { get; private set; }
    public Guid? SipId { get; private set; }
    public TransactionType Type { get; private set; }
    public Money Amount { get; private set; }
    public decimal Nav { get; private set; }
    public decimal Units { get; private set; }
    public DateOnly TransactionDate { get; private set; }

    public User User { get; private set; } = null!;
    public Scheme Scheme { get; private set; } = null!;
    public Sip? Sip { get; private set; }

    public static Transaction Record(
        Guid userId,
        Guid schemeId,
        Guid? sipId,
        TransactionType type,
        Money amount,
        decimal nav,
        DateOnly transactionDate)
    {
        if (amount <= Money.Zero)
        {
            throw new DomainException("Transaction amount must be greater than zero.");
        }

        if (nav <= 0)
        {
            throw new DomainException("NAV must be greater than zero.");
        }

        if (type == TransactionType.SipInstallment && sipId is null)
        {
            throw new DomainException("A SIP installment must reference a SIP.");
        }

        return new Transaction
        {
            UserId = userId,
            SchemeId = schemeId,
            SipId = sipId,
            Type = type,
            Amount = amount,
            Nav = nav,
            // Truncated so the investor is never allotted more units than the money bought.
            Units = decimal.Round(amount.Amount / nav, UnitDecimals, MidpointRounding.ToZero),
            TransactionDate = transactionDate,
        };
    }
}
