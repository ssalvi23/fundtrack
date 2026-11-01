using FundTrack.Domain.Entities;
using FundTrack.Infrastructure.Persistence.Seed;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FundTrack.Infrastructure.Persistence.Configurations;

internal sealed class SchemeConfiguration : IEntityTypeConfiguration<Scheme>
{
    public void Configure(EntityTypeBuilder<Scheme> builder)
    {
        builder.Property(scheme => scheme.Code).HasMaxLength(20).IsRequired();
        builder.Property(scheme => scheme.Name).HasMaxLength(200).IsRequired();
        builder.Property(scheme => scheme.Amc).HasMaxLength(100).IsRequired();
        builder.Property(scheme => scheme.Category).HasConversion<string>().HasMaxLength(20);
        builder.Property(scheme => scheme.RiskLevel).HasConversion<string>().HasMaxLength(20);
        builder.Property(scheme => scheme.CreatedBy).HasMaxLength(256).IsRequired();
        builder.Property(scheme => scheme.ModifiedBy).HasMaxLength(256);
        builder.Property(scheme => scheme.DeletedBy).HasMaxLength(256);

        builder.HasIndex(scheme => scheme.Code).IsUnique().HasFilter("[IsDeleted] = 0");
        builder.HasIndex(scheme => scheme.Name);
        builder.HasIndex(scheme => new { scheme.Category, scheme.RiskLevel });

        builder.HasData(SchemeSeed.All);
    }
}
