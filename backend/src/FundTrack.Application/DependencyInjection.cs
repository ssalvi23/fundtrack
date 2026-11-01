using Microsoft.Extensions.DependencyInjection;

namespace FundTrack.Application;

public static class DependencyInjection
{
    /// <summary>Registers use-case services. MediatR and validators are added in Phase 3.</summary>
    public static IServiceCollection AddApplication(this IServiceCollection services) => services;
}
