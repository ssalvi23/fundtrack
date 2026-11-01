using FundTrack.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FundTrack.Infrastructure.Persistence.Configurations;

internal sealed class SipConfiguration : IEntityTypeConfiguration<Sip>
{
    public void Configure(EntityTypeBuilder<Sip> builder)
    {
        builder.Property(sip => sip.Status).HasConversion<string>().HasMaxLength(20);
        builder.Property(sip => sip.CreatedBy).HasMaxLength(256).IsRequired();
        builder.Property(sip => sip.ModifiedBy).HasMaxLength(256);

        // Restrict: financial history must never vanish through a cascade.
        builder.HasOne(sip => sip.User).WithMany().HasForeignKey(sip => sip.UserId).OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(sip => sip.Scheme).WithMany().HasForeignKey(sip => sip.SchemeId).OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(sip => new { sip.UserId, sip.Status });
        // Supports the job that finds SIPs due today, in Phase 10.
        builder.HasIndex(sip => new { sip.Status, sip.NextInstallmentDate });
    }
}
