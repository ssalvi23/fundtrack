using FundTrack.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FundTrack.Infrastructure.Persistence.Configurations;

internal sealed class AuditLogConfiguration : IEntityTypeConfiguration<AuditLog>
{
    public void Configure(EntityTypeBuilder<AuditLog> builder)
    {
        builder.Property(log => log.EntityName).HasMaxLength(100).IsRequired();
        builder.Property(log => log.Action).HasMaxLength(20).IsRequired();
        builder.Property(log => log.ChangedBy).HasMaxLength(256).IsRequired();

        builder.HasIndex(log => new { log.EntityName, log.EntityId });
        builder.HasIndex(log => log.ChangedAtUtc);
    }
}
