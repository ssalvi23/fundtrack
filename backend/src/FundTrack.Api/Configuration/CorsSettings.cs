using System.ComponentModel.DataAnnotations;

namespace FundTrack.Api.Configuration;

/// <summary>Strongly typed "Cors" section. Named Settings to avoid clashing with ASP.NET's CorsOptions.</summary>
public sealed class CorsSettings
{
    public const string SectionName = "Cors";
    public const string PolicyName = "WebClient";

    [Required, MinLength(1)]
    public string[] AllowedOrigins { get; init; } = [];
}
