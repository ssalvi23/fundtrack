using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FundTrack.Infrastructure;

public static class DependencyInjection
{
    /// <summary>Registers external concerns. EF Core and SQL Server are added in Phase 2.</summary>
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration) => services;
}
