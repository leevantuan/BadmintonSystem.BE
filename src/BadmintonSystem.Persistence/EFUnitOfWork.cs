using BadmintonSystem.Domain.Abstractions;
using BadmintonSystem.Domain.Abstractions.Entities;
using Microsoft.EntityFrameworkCore;

namespace BadmintonSystem.Persistence;
public class EFUnitOfWork : IUnitOfWork
{
    private readonly ApplicationDbContext _context;

    public EFUnitOfWork(ApplicationDbContext context)
        => _context = context;

    public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        foreach (var entry in _context.ChangeTracker.Entries<AuditableEntity<Guid>>()
            .Where(q => q.State == EntityState.Added || q.State == EntityState.Modified))
        {
            entry.Entity.DateModified = DateTime.Now;

            if (entry.State == EntityState.Added)
            {
                entry.Entity.DateCreated = DateTime.Now;
            }
        }

        await _context.SaveChangesAsync(cancellationToken);
    }

    async ValueTask IAsyncDisposable.DisposeAsync()
        => await _context.DisposeAsync();
}
