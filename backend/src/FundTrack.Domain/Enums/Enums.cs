namespace FundTrack.Domain.Enums;

public enum UserRole
{
    User,
    Admin,
}

public enum SchemeCategory
{
    Equity,
    Debt,
    Hybrid,
    Liquid,
    Index,
}

/// <summary>The six levels of the SEBI riskometer.</summary>
public enum RiskLevel
{
    Low,
    LowToModerate,
    Moderate,
    ModeratelyHigh,
    High,
    VeryHigh,
}

public enum SipStatus
{
    Active,
    Paused,
    Cancelled,
}

public enum TransactionType
{
    SipInstallment,
    Lumpsum,
    Redemption,
}
