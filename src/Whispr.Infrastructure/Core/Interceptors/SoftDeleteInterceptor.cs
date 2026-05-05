using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Whispr.Domain.Core.Interfaces;

namespace Whispr.Infrastructure.Core.Interceptors;

public sealed class SoftDeleteInterceptor : SaveChangesInterceptor, ISingletonInterceptor
{
    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
        DbContextEventData eventData,
        InterceptionResult<int> result,
        CancellationToken cancellationToken = default)
    {
        if (eventData.Context is null)
        {
            return base.SavingChangesAsync(eventData, result, cancellationToken);
        }

        IEnumerable<EntityEntry<ISoftDeletable>> entries =
            eventData
                .Context
                .ChangeTracker
                .Entries<ISoftDeletable>()
                .Where(e => e.State == EntityState.Deleted);

        foreach (EntityEntry<ISoftDeletable> softDeletable in entries)
        {
            softDeletable.Entity.Delete();

            softDeletable.State = EntityState.Unchanged;
            softDeletable.Property(nameof(ISoftDeletable.IsDeleted)).IsModified = true;
            softDeletable.Property(nameof(ISoftDeletable.DeletedAtUtc)).IsModified = true;

            foreach (var reference in softDeletable.References)
            {
                reference.TargetEntry?.State = EntityState.Unchanged;
            }
        }

        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }
}