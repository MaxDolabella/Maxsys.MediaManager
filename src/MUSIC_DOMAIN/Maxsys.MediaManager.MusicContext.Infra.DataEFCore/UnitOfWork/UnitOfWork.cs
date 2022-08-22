using FluentValidation.Results;
using Maxsys.DataCore.Interfaces;
using Maxsys.MediaManager.MusicContext.Infra.DataEFCore.Context;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Logging;

namespace Maxsys.MediaManager.MusicContext.Infra.DataEFCore;

public class UnitOfWork : IUnitOfWork
{
    private readonly ILogger _logger;
    private readonly DbContext _context;

    #region PROPERTIES

    private IDbContextTransaction? Transaction { get; set; }

    public Guid Id { get; }
    public Guid ContextId { get; }

    #endregion PROPERTIES

    public UnitOfWork(ILogger<UnitOfWork> logger, MusicAppContext context)
    {
        Id = Guid.NewGuid();
        ContextId = context.ContextId.InstanceId;
        _logger = logger;
        _context = context;
    }

    public async ValueTask BeginTransactionAsync(CancellationToken token = default)
    {
        _logger.LogDebug("BeginTransaction()");

        Transaction = _context.Database.CurrentTransaction ?? await _context.Database.BeginTransactionAsync(token);
    }

    public async Task<ValidationResult> CommitAsync(CancellationToken token = default)
    {
        var result = new ValidationResult();

        // Create transaction before use it.
        await BeginTransactionAsync(token);
        using (Transaction)
        {
            try
            {
                _logger.LogDebug("Saving Changes async");
                int changes = await _context.SaveChangesAsync(token);
                _logger.LogInformation("Changes[{changes}] saved async.", changes);

                _logger.LogDebug("Committing async");
                await Transaction!.CommitAsync(token);
                _logger.LogInformation("Committed async");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Saving async or Committed async has failed.");
                result.AddException(ex, $"Saving async or Committed async has failed: {ex.Message}");

                await RollbackAsync(token);
            }
        }

        return result;
    }

    public async ValueTask RollbackAsync(CancellationToken token = default)
    {
        _logger.LogWarning("Executing RollbackAsync()");

        if (Transaction is not null)
            await Transaction.RollbackAsync(token);

        // manual ???
        // RollBackDBContext()

        _logger.LogWarning("RollbackAsync() Executed");
    }

    private void RollBackDBContext()
    {
        var changedEntries = _context.ChangeTracker.Entries()
            .Where(x => x.State != EntityState.Unchanged); //.ToList();

        foreach (var entry in changedEntries)
        {
            switch (entry.State)
            {
                case EntityState.Modified:
                    entry.CurrentValues.SetValues(entry.OriginalValues);
                    entry.State = EntityState.Unchanged;
                    break;

                case EntityState.Added:
                    entry.State = EntityState.Detached;
                    break;

                case EntityState.Deleted:
                    entry.State = EntityState.Unchanged;
                    break;
            }
        }
    }

    #region DIPOSABLE IMPLEMENTATION

    protected bool _disposed = false;

    protected virtual void Dispose(bool disposing)
    {
        if (!_disposed)
        {
            if (disposing)
            {
                Transaction?.Dispose();
            }
        }
        _disposed = true;
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    #endregion DIPOSABLE IMPLEMENTATION
}