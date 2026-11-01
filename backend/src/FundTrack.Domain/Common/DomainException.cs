namespace FundTrack.Domain.Common;

/// <summary>Thrown when a business rule would be broken. Mapped to an HTTP error in Phase 3.</summary>
public sealed class DomainException(string message) : Exception(message);
