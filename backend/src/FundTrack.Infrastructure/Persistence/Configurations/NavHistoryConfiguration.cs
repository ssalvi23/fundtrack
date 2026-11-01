using FundTrack.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FundTrack.Infrastructure.Persistence.Configurations;

internal sealed class NavHistoryConfiguration : IEntityTypeConfiguration<NavHistory>
{
    public void Configure(EntityTypeBuilder<NavHistory> builder)
    {
        builder.ToTable("NavHistory");

        builder.HasOne<Scheme>().WithMany().HasForeignKey(history => history.SchemeId).OnDelete(DeleteBehavior.Restrict);

        // One NAV per scheme per day. Also makes re-running the daily job safe.
        builder.HasIndex(history => new { history.SchemeId, history.NavDate }).IsUnique();
    }
}
