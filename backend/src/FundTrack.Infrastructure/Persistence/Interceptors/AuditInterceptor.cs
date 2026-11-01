using System.Text.Json;
using System.Text.Json.Serialization;
using FundTrack.Application.Abstractions;
using FundTrack.Domain.Common;
using FundTrack.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace FundTrack.Infrastructure.Persistence.Interceptors;

/// <summary>
/// Runs inside every SaveChanges: stamps audit fields, turns deletes into soft deletes,
/// and writes AuditLog rows in the same database transaction as the change itself.
/// </summary>
internal sealed class AuditInterceptor(ICurrentUser currentUser, TimeProvider timeProvider) : SaveChangesInterceptor
{
    // Secrets must never be copied into a log table.
    private static readonly HashSet<string> ExcludedProperties = [nameof(User.PasswordHash)];

    // "Paused" reads better than 1 when someone investigates this log years later.
    private static readonly JsonSerializerOptions JsonOptions = new() { Converters = { new JsonStringEnumConverter() } };

    public override InterceptionResult<int> SavingChanges(
        DbContextEventData eventData,
        InterceptionResult<int> result)
    {
        ApplyAudit(eventData.Context);
        return result;
    }

    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
        DbContextEventData eventData,
        InterceptionResult<int> result,
        CancellationToken cancellationToken = default)
    {
        ApplyAudit(eventData.Context);
        return ValueTask.FromResult(result);
    }

    private void ApplyAudit(DbContext? context)
    {
        if (context is null)
        {
            return;
        }

        var now = timeProvider.GetUtcNow();
        var user = currentUser.Name;

        // ToList: adding AuditLog rows below would otherwise change the collection being read.
        var entries = context.ChangeTracker.Entries<AuditableEntity>()
            .Where(entry => entry.State is EntityState.Added or EntityState.Modified or EntityState.Deleted)
            .ToList();

        foreach (var entry in entries)
        {
            var action = entry.State.ToString();
            var isSoftDelete = entry is { State: EntityState.Deleted, Entity: ISoftDeletable };

            if (isSoftDelete)
            {
                // Unchanged, not Modified: otherwise EF would rewrite every column.
                entry.State = EntityState.Unchanged;
                entry.Property(nameof(ISoftDeletable.IsDeleted)).CurrentValue = true;
                entry.Property(nameof(ISoftDeletable.DeletedAtUtc)).CurrentValue = now;
                entry.Property(nameof(ISoftDeletable.DeletedBy)).CurrentValue = user;
            }

            var (oldValues, newValues) = CaptureChanges(entry);

            if (action == nameof(EntityState.Added))
            {
                entry.Property(nameof(AuditableEntity.CreatedAtUtc)).CurrentValue = now;
                entry.Property(nameof(AuditableEntity.CreatedBy)).CurrentValue = user;
            }
            else if (entry.State == EntityState.Modified)
            {
                entry.Property(nameof(AuditableEntity.ModifiedAtUtc)).CurrentValue = now;
                entry.Property(nameof(AuditableEntity.ModifiedBy)).CurrentValue = user;
            }

            context.Set<AuditLog>().Add(AuditLog.Record(
                entry.Metadata.ClrType.Name,
                entry.Entity.Id,
                action,
                oldValues,
                newValues,
                user,
                now));
        }
    }

    private static (string? OldValues, string? NewValues) CaptureChanges(EntityEntry entry)
    {
        var properties = entry.Properties
            .Where(property => !ExcludedProperties.Contains(property.Metadata.Name))
            .ToList();

        return entry.State switch
        {
            EntityState.Added => (null, Serialize(properties, property => property.CurrentValue)),
            EntityState.Deleted => (Serialize(properties, property => property.OriginalValue), null),
            _ => (
                Serialize(properties.Where(property => property.IsModified), property => property.OriginalValue),
                Serialize(properties.Where(property => property.IsModified), property => property.CurrentValue)),
        };
    }

    private static string Serialize(IEnumerable<PropertyEntry> properties, Func<PropertyEntry, object?> value) =>
        JsonSerializer.Serialize(properties.ToDictionary(property => property.Metadata.Name, value), JsonOptions);
}
