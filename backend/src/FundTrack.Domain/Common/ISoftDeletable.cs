namespace FundTrack.Domain.Common;

/// <summary>Marks an entity whose rows are hidden rather than removed when deleted.</summary>
public interface ISoftDeletable
{
    bool IsDeleted { get; }
    DateTimeOffset? DeletedAtUtc { get; }
    string? DeletedBy { get; }
}
