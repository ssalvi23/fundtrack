using System.ComponentModel.DataAnnotations;

namespace FundTrack.Infrastructure.Persistence;

public sealed class DatabaseSettings
{
    public const string SectionName = "Database";

    [Required]
    public string ConnectionString { get; init; } = string.Empty;
}
