using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;

namespace FundTrack.IntegrationTests;

/// <summary>Supplies the required settings so tests run on any machine, without user secrets.</summary>
public sealed class FundTrackApiFactory : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Development");
        // These tests never open a connection. Real database tests arrive with Testcontainers in Phase 12.
        builder.UseSetting("Database:ConnectionString", "Server=unused;Database=unused");
    }
}
