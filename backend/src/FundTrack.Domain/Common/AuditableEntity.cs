namespace FundTrack.Domain.Common;

/// <summary>Private setters: only the audit interceptor writes these, through EF Core's change tracker.</summary>
public abstract class AuditableEntity : Entity
{
    public DateTimeOffset CreatedAtUtc { get; private set; }
    public string CreatedBy { get; private set; } = null!;
    public DateTimeOffset? ModifiedAtUtc { get; private set; }
    public string? ModifiedBy { get; private set; }
}
