using Maxsys.MediaManager.CoreDomain.Interfaces;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace Maxsys.MediaManager.MusicContext.Infra.DataEFCore.Context.Interceptors;

public sealed class AuditableEntityInterceptor : SaveChangesInterceptor
{
    public override async ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default)
    {
        var entries = eventData.Context!.ChangeTracker
            .Entries<IAuditableEntity>()
            .Where(e => e.State is EntityState.Added or EntityState.Modified)
            .ToList();

        foreach (var entry in entries)
        {
            if (entry.State is EntityState.Added)
            {
                entry.Entity.SetCreatedAt(DateTime.Now);
            }
            else //if (entry.State is EntityState.Modified)
            {
                entry.Entity.SetLastUpdateAt(DateTime.Now);
            }
        }

        return await base.SavingChangesAsync(eventData, result, cancellationToken);
    }
}