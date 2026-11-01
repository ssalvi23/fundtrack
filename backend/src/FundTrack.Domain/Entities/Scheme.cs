using FundTrack.Domain.Common;
using FundTrack.Domain.Enums;

namespace FundTrack.Domain.Entities;

public sealed class Scheme : AuditableEntity, ISoftDeletable
{
    private Scheme()
    {
    }

    public string Code { get; private set; } = null!;
    public string Name { get; private set; } = null!;
    public string Amc { get; private set; } = null!;
    public SchemeCategory Category { get; private set; }
    public RiskLevel RiskLevel { get; private set; }
    public decimal CurrentNav { get; private set; }
    public DateOnly NavDate { get; private set; }

    public bool IsDeleted { get; private set; }
    public DateTimeOffset? DeletedAtUtc { get; private set; }
    public string? DeletedBy { get; private set; }

    public static Scheme Create(
        string code,
        string name,
        string amc,
        SchemeCategory category,
        RiskLevel riskLevel,
        decimal nav,
        DateOnly navDate)
    {
        if (string.IsNullOrWhiteSpace(code))
        {
            throw new DomainException("Scheme code is required.");
        }

        if (string.IsNullOrWhiteSpace(name))
        {
            throw new DomainException("Scheme name is required.");
        }

        if (string.IsNullOrWhiteSpace(amc))
        {
            throw new DomainException("AMC is required.");
        }

        var scheme = new Scheme
        {
            Code = code.Trim(),
            Name = name.Trim(),
            Amc = amc.Trim(),
            Category = category,
            RiskLevel = riskLevel,
        };
        scheme.UpdateNav(nav, navDate);
        return scheme;
    }

    public void UpdateNav(decimal nav, DateOnly navDate)
    {
        if (nav <= 0)
        {
            throw new DomainException("NAV must be greater than zero.");
        }

        if (navDate < NavDate)
        {
            throw new DomainException("NAV date cannot be older than the current NAV date.");
        }

        CurrentNav = nav;
        NavDate = navDate;
    }
}
