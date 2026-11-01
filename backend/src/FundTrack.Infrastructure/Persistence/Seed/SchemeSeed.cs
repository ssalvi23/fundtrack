using FundTrack.Domain.Enums;

namespace FundTrack.Infrastructure.Persistence.Seed;

/// <summary>
/// Fictional sample schemes. Ids and dates are fixed because EF Core compares seed data
/// between migrations: a Guid.NewGuid() here would produce a new migration every time.
/// </summary>
internal static class SchemeSeed
{
    private static readonly DateOnly NavDate = new(2026, 10, 30);
    private static readonly DateTimeOffset SeededAt = new(2026, 11, 1, 0, 0, 0, TimeSpan.Zero);

    public static readonly object[] All =
    [
        Scheme("10000000-0000-0000-0000-000000000001", "FT100001", "Aarav Flexi Cap Fund", "Aarav Mutual Fund", SchemeCategory.Equity, RiskLevel.VeryHigh, 78.4521m),
        Scheme("10000000-0000-0000-0000-000000000002", "FT100002", "Aarav Large Cap Fund", "Aarav Mutual Fund", SchemeCategory.Equity, RiskLevel.High, 52.1187m),
        Scheme("10000000-0000-0000-0000-000000000003", "FT100003", "Aarav Short Duration Fund", "Aarav Mutual Fund", SchemeCategory.Debt, RiskLevel.LowToModerate, 31.6642m),
        Scheme("10000000-0000-0000-0000-000000000004", "FT100004", "Nirmal Small Cap Fund", "Nirmal Mutual Fund", SchemeCategory.Equity, RiskLevel.VeryHigh, 112.9035m),
        Scheme("10000000-0000-0000-0000-000000000005", "FT100005", "Nirmal Balanced Advantage Fund", "Nirmal Mutual Fund", SchemeCategory.Hybrid, RiskLevel.ModeratelyHigh, 44.2876m),
        Scheme("10000000-0000-0000-0000-000000000006", "FT100006", "Nirmal Liquid Fund", "Nirmal Mutual Fund", SchemeCategory.Liquid, RiskLevel.Low, 2543.1098m),
        Scheme("10000000-0000-0000-0000-000000000007", "FT100007", "Sahyadri Nifty 50 Index Fund", "Sahyadri Mutual Fund", SchemeCategory.Index, RiskLevel.High, 26.7314m),
        Scheme("10000000-0000-0000-0000-000000000008", "FT100008", "Sahyadri Corporate Bond Fund", "Sahyadri Mutual Fund", SchemeCategory.Debt, RiskLevel.Moderate, 28.0459m),
    ];

    private static object Scheme(
        string id,
        string code,
        string name,
        string amc,
        SchemeCategory category,
        RiskLevel riskLevel,
        decimal nav) => new
        {
            Id = Guid.Parse(id),
            Code = code,
            Name = name,
            Amc = amc,
            Category = category,
            RiskLevel = riskLevel,
            CurrentNav = nav,
            NavDate,
            IsDeleted = false,
            CreatedAtUtc = SeededAt,
            CreatedBy = "seed",
        };
}
