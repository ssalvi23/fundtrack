using FundTrack.Domain.Common;
using FundTrack.Domain.ValueObjects;
using Shouldly;

namespace FundTrack.UnitTests.Domain;

public sealed class MoneyTests
{
    [Fact]
    public void Constructor_RejectsNegativeAmounts() =>
        Should.Throw<DomainException>(() => new Money(-0.01m));

    [Theory]
    [InlineData(10.005, 10.01)]
    [InlineData(10.004, 10.00)]
    public void Constructor_RoundsToTwoDecimals(decimal input, decimal expected) =>
        new Money(input).Amount.ShouldBe(expected);

    [Fact]
    public void Addition_HasNoFloatingPointError() =>
        (new Money(0.10m) + new Money(0.20m)).Amount.ShouldBe(0.30m);

    [Fact]
    public void Subtraction_BelowZeroIsRejected() =>
        Should.Throw<DomainException>(() => new Money(1m) - new Money(2m));

    [Fact]
    public void Equality_ComparesByValue() =>
        new Money(500m).ShouldBe(new Money(500.00m));
}
