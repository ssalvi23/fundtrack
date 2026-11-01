using FundTrack.Domain.Common;
using FundTrack.Domain.Entities;
using FundTrack.Domain.Enums;
using FundTrack.Domain.ValueObjects;
using Shouldly;

namespace FundTrack.UnitTests.Domain;

public sealed class TransactionTests
{
    private static readonly DateOnly Date = new(2026, 11, 1);

    [Fact]
    public void Record_TruncatesUnitsToThreeDecimals()
    {
        // 1000 / 78.4521 = 12.74630...
        var transaction = Transaction.Record(
            Guid.NewGuid(), Guid.NewGuid(), null, TransactionType.Lumpsum, new Money(1000m), 78.4521m, Date);

        transaction.Units.ShouldBe(12.746m);
    }

    [Fact]
    public void Record_SipInstallmentWithoutSip_IsRejected() => Should.Throw<DomainException>(() =>
        Transaction.Record(
            Guid.NewGuid(), Guid.NewGuid(), null, TransactionType.SipInstallment, new Money(1000m), 50m, Date));

    [Fact]
    public void Record_WithZeroAmount_IsRejected() => Should.Throw<DomainException>(() =>
        Transaction.Record(Guid.NewGuid(), Guid.NewGuid(), null, TransactionType.Lumpsum, Money.Zero, 50m, Date));
}
