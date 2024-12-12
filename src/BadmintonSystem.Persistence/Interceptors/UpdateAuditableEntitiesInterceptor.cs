using BadmintonSystem.Contract.Abstractions.Entities;
using BadmintonSystem.Persistence.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace BadmintonSystem.Persistence.Interceptors;

public class UpdateAuditableEntitiesInterceptor(IHttpContextAccessor httpContextAccessor) : SaveChangesInterceptor
{
    public override ValueTask<InterceptionResult<int>> SavingChangesAsync
    (
        DbContextEventData eventData,
        InterceptionResult<int> result,
        CancellationToken cancellationToken = default)
    {
        if (eventData.Context is not null)
        {
            UpdateAuditableEntities(eventData.Context, httpContextAccessor);
        }

        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }

    private static void UpdateAuditableEntities(DbContext context, IHttpContextAccessor httpContextAccessor)
    {
        DateTime datetimeNow = DateTime.Now;
        Guid currentUserId = httpContextAccessor.HttpContext?.GetCurrentUserId() ?? Guid.Empty;

        var entities = context.ChangeTracker.Entries().ToList();

        foreach (EntityEntry entry in entities)
        {
            if (entry.Entity is IDateTracking dateTracking)
            {
                UpdateDateTracking(entry, dateTracking, datetimeNow);
            }

            if (entry.Entity is IUserTracking userTracking)
            {
                UpdateUserTracking(entry, userTracking, currentUserId);
            }

            if (entry.Entity is ISoftDelete softDelete)
            {
                UpdateSoftDelete(entry, softDelete, datetimeNow);
            }
        }
    }

    private static void UpdateDateTracking(EntityEntry entry, IDateTracking entity, DateTime datetimeNow)
    {
        if (entry.State == EntityState.Added)
        {
            entity.CreatedDate = datetimeNow;
        }

        if (entry.State == EntityState.Modified)
        {
            entity.ModifiedDate = datetimeNow;
        }
    }

    private static void UpdateUserTracking(EntityEntry entry, IUserTracking entity, Guid currentUserId)
    {
        if (entry.State == EntityState.Added)
        {
            entity.CreatedBy = currentUserId;
        }

        if (entry.State == EntityState.Modified)
        {
            entity.ModifiedBy = currentUserId;
        }
    }

    private static void UpdateSoftDelete(EntityEntry entry, ISoftDelete entity, DateTime datetimeNow)
    {
        if (entry.State == EntityState.Added)
        {
            entity.IsDeleted = false;
        }

        if (entry.Entity is IHardDelete)
        {
            return;
        }

        if (entry.State == EntityState.Deleted)
        {
            entry.State = EntityState.Modified;
            entity.IsDeleted = true;
            entity.DeletedAt = datetimeNow;
        }
    }

    private static void SetCurrentPropertyValue(EntityEntry entry, string propertyName, DateTime datetimeNow)
    {
        entry.Property(propertyName).CurrentValue = datetimeNow;
    }
}
