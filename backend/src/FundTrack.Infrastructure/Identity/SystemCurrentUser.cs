using FundTrack.Application.Abstractions;

namespace FundTrack.Infrastructure.Identity;

/// <summary>Used until Phase 4 adds real logins, and later for background jobs.</summary>
internal sealed class SystemCurrentUser : ICurrentUser
{
    public string Name => "system";
}
