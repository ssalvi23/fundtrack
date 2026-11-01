using FundTrack.Application.Abstractions;
using FundTrack.Infrastructure.Identity;
using FundTrack.Infrastructure.Persistence;
using FundTrack.Infrastructure.Persistence.Interceptors;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace FundTrack.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddOptions<DatabaseSettings>()
            .Bind(configuration.GetSection(DatabaseSettings.SectionName))
            .ValidateDataAnnotations()
            .ValidateOnStart();

        services.AddSingleton(TimeProvider.System);
        services.AddSingleton<ICurrentUser, SystemCurrentUser>();
        services.AddScoped<AuditInterceptor>();

        services.AddDbContext<FundTrackDbContext>((provider, options) => options
            .UseSqlServer(provider.GetRequiredService<IOptions<DatabaseSettings>>().Value.ConnectionString)
            .AddInterceptors(provider.GetRequiredService<AuditInterceptor>()));

        return services;
    }
}
