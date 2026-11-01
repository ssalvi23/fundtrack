using System.Net;
using System.Net.Http.Json;

namespace FundTrack.IntegrationTests;

public sealed class HealthEndpointTests(FundTrackApiFactory factory)
    : IClassFixture<FundTrackApiFactory>
{
    private const string AllowedOrigin = "http://localhost:4200";

    [Fact]
    public async Task Health_ReturnsHealthyJson()
    {
        var client = factory.CreateClient();

        var response = await client.GetAsync("/health");
        var body = await response.Content.ReadFromJsonAsync<HealthBody>();

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Equal("Healthy", body?.Status);
    }

    [Theory]
    [InlineData(AllowedOrigin, true)]
    [InlineData("https://evil.example", false)]
    public async Task Health_OnlyAllowsConfiguredOrigins(string origin, bool expectAllowed)
    {
        var client = factory.CreateClient();
        using var request = new HttpRequestMessage(HttpMethod.Get, "/health");
        request.Headers.Add("Origin", origin);

        var response = await client.SendAsync(request);

        Assert.Equal(expectAllowed, response.Headers.Contains("Access-Control-Allow-Origin"));
    }

    private sealed record HealthBody(string Status);
}
