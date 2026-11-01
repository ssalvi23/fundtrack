namespace FundTrack.Domain.Common;

public abstract class Entity
{
    // Left empty so EF Core assigns a sequential GUID, which fragments the clustered index far less.
    public Guid Id { get; protected set; }
}
