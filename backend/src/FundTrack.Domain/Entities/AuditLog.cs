using FundTrack.Domain.Common;

namespace FundTrack.Domain.Entities;

/// <summary>Who changed what and when. Append-only: rows are never updated or deleted.</summary>
public sealed class AuditLog : Entity
{
    private AuditLog()
    {
    }

    public string EntityName { get; private set; } = null!;
    public Guid EntityId { get; private set; }
    public string Action { get; private set; } = null!;
    public string? OldValues { get; private set; }
    public string? NewValues { get; private set; }
    public string ChangedBy { get; private set; } = null!;
    public DateTimeOffset ChangedAtUtc { get; private set; }

    public static AuditLog Record(
        string entityName,
        Guid entityId,
        string action,
        string? oldValues,
        string? newValues,
        string changedBy,
        DateTimeOffset changedAtUtc) => new()
        {
            EntityName = entityName,
            EntityId = entityId,
            Action = action,
            OldValues = oldValues,
            NewValues = newValues,
            ChangedBy = changedBy,
            ChangedAtUtc = changedAtUtc,
        };
}
