using FundTrack.Domain.Common;
using FundTrack.Domain.Enums;
using FundTrack.Domain.ValueObjects;

namespace FundTrack.Domain.Entities;

public sealed class Sip : AuditableEntity
{
    // Capped at 28 so every month, including February, has a valid installment date.
    public const int MaxDayOfMonth = 28;
    public static readonly Money MinimumInstallment = new(100m);

    private Sip()
    {
    }

    public Guid UserId { get; private set; }
    public Guid SchemeId { get; private set; }
    public Money InstallmentAmount { get; private set; }
    public int DayOfMonth { get; private set; }
    public DateOnly StartDate { get; private set; }
    public DateOnly? NextInstallmentDate { get; private set; }
    public SipStatus Status { get; private set; }

    public User User { get; private set; } = null!;
    public Scheme Scheme { get; private set; } = null!;

    public static Sip Start(Guid userId, Guid schemeId, Money installmentAmount, int dayOfMonth, DateOnly startDate)
    {
        if (installmentAmount < MinimumInstallment)
        {
            throw new DomainException($"SIP installment must be at least {MinimumInstallment}.");
        }

        if (dayOfMonth is < 1 or > MaxDayOfMonth)
        {
            throw new DomainException($"SIP day must be between 1 and {MaxDayOfMonth}.");
        }

        return new Sip
        {
            UserId = userId,
            SchemeId = schemeId,
            InstallmentAmount = installmentAmount,
            DayOfMonth = dayOfMonth,
            StartDate = startDate,
            NextInstallmentDate = NextOccurrence(dayOfMonth, startDate),
            Status = SipStatus.Active,
        };
    }

    public void Pause()
    {
        if (Status != SipStatus.Active)
        {
            throw new DomainException($"Only an active SIP can be paused. Current status: {Status}.");
        }

        Status = SipStatus.Paused;
        NextInstallmentDate = null;
    }

    public void Resume(DateOnly today)
    {
        if (Status != SipStatus.Paused)
        {
            throw new DomainException($"Only a paused SIP can be resumed. Current status: {Status}.");
        }

        Status = SipStatus.Active;
        NextInstallmentDate = NextOccurrence(DayOfMonth, today);
    }

    public void Cancel()
    {
        if (Status == SipStatus.Cancelled)
        {
            throw new DomainException("SIP is already cancelled.");
        }

        Status = SipStatus.Cancelled;
        NextInstallmentDate = null;
    }

    private static DateOnly NextOccurrence(int dayOfMonth, DateOnly from)
    {
        var candidate = new DateOnly(from.Year, from.Month, dayOfMonth);
        return candidate >= from ? candidate : candidate.AddMonths(1);
    }
}
