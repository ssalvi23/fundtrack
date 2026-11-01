using FundTrack.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FundTrack.Infrastructure.Persistence.Configurations;

internal sealed class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.Property(user => user.Email).HasMaxLength(256).IsRequired();
        builder.Property(user => user.FullName).HasMaxLength(150).IsRequired();
        builder.Property(user => user.PasswordHash).HasMaxLength(500).IsRequired();
        builder.Property(user => user.Role).HasConversion<string>().HasMaxLength(20);
        builder.Property(user => user.CreatedBy).HasMaxLength(256).IsRequired();
        builder.Property(user => user.ModifiedBy).HasMaxLength(256);
        builder.Property(user => user.DeletedBy).HasMaxLength(256);

        // Filtered: a soft-deleted account must not block the same email registering again.
        builder.HasIndex(user => user.Email).IsUnique().HasFilter("[IsDeleted] = 0");
    }
}
