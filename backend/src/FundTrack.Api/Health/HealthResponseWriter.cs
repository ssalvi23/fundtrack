using System.Text.Json;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace FundTrack.Api.Health;

/// <summary>Writes a small JSON body instead of the default plain-text "Healthy".</summary>
public static class HealthResponseWriter
{
    private static readonly JsonSerializerOptions JsonOptions = new(JsonSerializerDefaults.Web);

    public static Task WriteAsync(HttpContext context, HealthReport report)
    {
        var response = new HealthResponse(
            report.Status.ToString(),
            context.RequestServices.GetRequiredService<IHostEnvironment>().EnvironmentName,
            DateTimeOffset.UtcNow);

        return context.Response.WriteAsJsonAsync(
            response,
            JsonOptions,
            contentType: "application/json",
            cancellationToken: context.RequestAborted);
    }

    private sealed record HealthResponse(string Status, string Environment, DateTimeOffset CheckedAtUtc);
}
