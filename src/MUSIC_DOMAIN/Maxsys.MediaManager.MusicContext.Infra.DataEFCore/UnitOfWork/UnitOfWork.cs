using FluentValidation.Results;
using Maxsys.DataCore.Interfaces;
using Maxsys.MediaManager.MusicContext.Infra.DataEFCore.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Maxsys.MediaManager.MusicContext.Infra.DataEFCore
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly DbContext _context;
        private readonly ILogger<UnitOfWork> _logger;

        private IDbContextTransaction Transaction
            => _context.Database.CurrentTransaction
            ?? _context.Database.BeginTransaction();

        public UnitOfWork(MusicAppContext context, ILogger<UnitOfWork> logger)
        {
            _context = context;
            _logger = logger;
        }

        public void BeginTransaction()
        {
            _logger.LogDebug("BeginTransaction()");
            //_ = Transaction;
            _disposed = false;
        }

        public ValidationResult SaveChanges()
        {
            //NOTE Transaction;
            var result = new ValidationResult();
            using (Transaction)
            {
                try
                {
                    _logger.LogDebug("Saving Changes");
                    _context.SaveChanges();
                    _logger.LogInformation("Changes saved.");

                    _logger.LogDebug("Committing");
                    Transaction.Commit();
                    _logger.LogInformation("Committed");
                }
                catch (Exception ex)
                {
                    _logger.LogError("Saving or Committed has failed. Error: {0}", ex);
                    result.AddFailure($"Saving or Committed has failed. Error: {ex.Message}");

                    RollBack();//_transaction.Rollback();
                }
            }

            return result;
        }

        public async Task<ValidationResult> SaveChangesAsync()
        {
            //NOTE Transaction;
            var result = new ValidationResult();
            using (Transaction)
            {
                try
                {
                    _logger.LogDebug("Saving Changes async");
                    int changes = await _context.SaveChangesAsync();
                    _logger.LogInformation($"Changes[{changes}] saved async.");

                    _logger.LogDebug("Committing async");
                    await Transaction.CommitAsync();
                    _logger.LogInformation("Committed async");
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Saving async or Committed async has failed.");
                    result.AddFailure($"Saving async or Committed async has failed: {ex.Message}");

                    await RollbackAsync();//_transaction.Rollback();
                }
            }

            return result;
        }

        public void RollBack()
        {
            _logger.LogWarning("Executing Rollback()");

            Transaction?.Rollback(); //_dbContext.Database.RollbackTransaction();

            RollBackDBContext();

            _logger.LogWarning("Rollback() Executed");
        }

        public async Task RollbackAsync()
        {
            _logger.LogWarning("Executing RollbackAsync()");

            await Transaction?.RollbackAsync();

            RollBackDBContext();

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
}