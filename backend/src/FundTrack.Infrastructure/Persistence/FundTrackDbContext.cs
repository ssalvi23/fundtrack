using System.Linq.Expressions;
using FundTrack.Domain.Common;
using FundTrack.Domain.Entities;
using FundTrack.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace FundTrack.Infrastructure.Persistence;

public sealed class FundTrackDbContext(DbContextOptions<FundTrackDbContext> options) : DbContext(options)
{
    public DbSet<User> Users => Set<User>();
    public DbSet<Scheme> Schemes => Set<Scheme>();
    public DbSet<Sip> Sips => Set<Sip>();
    public DbSet<Transaction> Transactions => Set<Transaction>();
    public DbSet<NavHistory> NavHistory => Set<NavHistory>();
    public DbSet<AuditLog> AuditLogs => Set<AuditLog>();

    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    {
        // Every decimal gets an explicit precision, because SQL Server's default of (18,2) would round NAVs.
        configurationBuilder.Properties<decimal>().HavePrecision(18, 4);
        configurationBuilder.Properties<Money>().HaveConversion<MoneyConverter>().HavePrecision(18, 2);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(FundTrackDbContext).Assembly);

        // Applied by convention so a new soft-deletable entity cannot forget its filter.
        foreach (var entityType in modelBuilder.Model.GetEntityTypes()
                     .Where(type => typeof(ISoftDeletable).IsAssignableFrom(type.ClrType)))
        {
            var parameter = Expression.Parameter(entityType.ClrType, "entity");
            var isDeleted = Expression.Property(parameter, nameof(ISoftDeletable.IsDeleted));
            entityType.SetQueryFilter(Expression.Lambda(Expression.Not(isDeleted), parameter));
        }
    }

    private sealed class MoneyConverter() : ValueConverter<Money, decimal>(
        money => money.Amount,
        amount => new Money(amount));
}
