using FundTrack.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FundTrack.Infrastructure.Persistence.Configurations;

internal sealed class TransactionConfiguration : IEntityTypeConfiguration<Transaction>
{
    public void Configure(EntityTypeBuilder<Transaction> builder)
    {
        builder.Property(transaction => transaction.Type).HasConversion<string>().HasMaxLength(20);
        // Registrars allot units to 3 decimal places, so this overrides the 4-place decimal default.
        builder.Property(transaction => transaction.Units).HasPrecision(18, 3);
        builder.Property(transaction => transaction.CreatedBy).HasMaxLength(256).IsRequired();
        builder.Property(transaction => transaction.ModifiedBy).HasMaxLength(256);

        builder.HasOne(transaction => transaction.User)
            .WithMany()
            .HasForeignKey(transaction => transaction.UserId)
            .OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(transaction => transaction.Scheme)
            .WithMany()
            .HasForeignKey(transaction => transaction.SchemeId)
            .OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(transaction => transaction.Sip)
            .WithMany()
            .HasForeignKey(transaction => transaction.SipId)
            .OnDelete(DeleteBehavior.Restrict);

        // Serves "my transactions, newest first", the most common query on this table.
        builder.HasIndex(transaction => new { transaction.UserId, transaction.TransactionDate })
            .IsDescending(false, true);
    }
}
