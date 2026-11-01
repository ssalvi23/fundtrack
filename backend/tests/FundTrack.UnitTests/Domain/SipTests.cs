using FundTrack.Domain.Common;
using FundTrack.Domain.Entities;
using FundTrack.Domain.Enums;
using FundTrack.Domain.ValueObjects;
using Shouldly;

namespace FundTrack.UnitTests.Domain;

public sealed class SipTests
{
    private static Sip NewSip(int day = 5, DateOnly? start = null) => Sip.Start(
        Guid.NewGuid(),
        Guid.NewGuid(),
        new Money(1000m),
        day,
        start ?? new DateOnly(2026, 11, 1));

    [Fact]
    public void Start_BelowMinimumInstallment_IsRejected() => Should.Throw<DomainException>(() =>
        Sip.Start(Guid.NewGuid(), Guid.NewGuid(), new Money(99.99m), 5, new DateOnly(2026, 11, 1)));

    [Theory]
    [InlineData(0)]
    [InlineData(29)]
    public void Start_WithInvalidDay_IsRejected(int day) =>
        Should.Throw<DomainException>(() => NewSip(day));

    [Fact]
    public void Start_WhenDayIsAhead_SchedulesThisMonth() =>
        NewSip(day: 5, new DateOnly(2026, 11, 1)).NextInstallmentDate.ShouldBe(new DateOnly(2026, 11, 5));

    [Fact]
    public void Start_WhenDayHasPassed_SchedulesNextMonth() =>
        NewSip(day: 5, new DateOnly(2026, 12, 20)).NextInstallmentDate.ShouldBe(new DateOnly(2027, 1, 5));

    [Fact]
    public void Pause_ClearsNextInstallment()
    {
        var sip = NewSip();

        sip.Pause();

        sip.Status.ShouldBe(SipStatus.Paused);
        sip.NextInstallmentDate.ShouldBeNull();
    }

    [Fact]
    public void Resume_ReschedulesFromToday()
    {
        var sip = NewSip();
        sip.Pause();

        sip.Resume(new DateOnly(2027, 2, 10));

        sip.Status.ShouldBe(SipStatus.Active);
        sip.NextInstallmentDate.ShouldBe(new DateOnly(2027, 3, 5));
    }

    [Fact]
    public void Resume_WhenActive_IsRejected() =>
        Should.Throw<DomainException>(() => NewSip().Resume(new DateOnly(2026, 11, 2)));

    [Fact]
    public void Cancel_IsFinal()
    {
        var sip = NewSip();
        sip.Cancel();

        Should.Throw<DomainException>(sip.Pause);
        Should.Throw<DomainException>(() => sip.Resume(new DateOnly(2026, 12, 1)));
        Should.Throw<DomainException>(sip.Cancel);
    }
}
