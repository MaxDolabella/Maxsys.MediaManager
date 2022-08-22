using Maxsys.ModelCore;
using Maxsys.ModelCore.Interfaces.Repositories;
using System.Linq.Expressions;

namespace Maxsys.MediaManager.MusicContext.Infra.DataEFCore.Repositories.Common;

// https://github.com/Arch/UnitOfWork/blob/master/src/UnitOfWork/Repository.cs
/// <inheritdoc/>
public abstract class RepositoryBase<TEntity> : IRepositoryBase<TEntity>
    where TEntity : EntityBase
{
    #region FIELDS

    protected readonly DbContext Context;
    protected readonly DbSet<TEntity> DbSet;

    #endregion FIELDS

    #region PROPERTIES

    public Guid Id { get; }
    public Guid ContextId { get; }

    #endregion PROPERTIES

    #region CONSTRUCTOR

    public RepositoryBase(DbContext dbContext)
    {
        Id = Guid.NewGuid();
        ContextId = dbContext.ContextId.InstanceId;
        Context = dbContext;
        DbSet = Context.Set<TEntity>();
    }

    #endregion CONSTRUCTOR

    #region CRUD ASYNCHRONOUS

    public virtual async ValueTask<bool> AddAsync(TEntity obj, CancellationToken token = default)
    {
        var entry = await DbSet.AddAsync(obj, token);

        return entry.State == EntityState.Added;
    }

    public virtual async ValueTask<bool> AddRangeAsync(IEnumerable<TEntity> entities, CancellationToken token = default)
    {
        // await DbSet.AddRangeAsync(entities, token);
        foreach (var item in entities)
        {
            if (!await AddAsync(item, token))
                return false;
        }

        return true;
    }

    public virtual async ValueTask<bool> UpdateAsync(TEntity obj, CancellationToken token = default)
    {
        if (token.IsCancellationRequested) return false;

        var entry = DbSet.Update(obj);

        return await Task.FromResult(entry.State == EntityState.Modified);
    }

    public virtual async ValueTask<bool> RemoveAsync(object id, CancellationToken token = default)
    {
        var entity = await DbSet.FindAsync(new object?[] { id }, cancellationToken: token);

        return entity is null || await RemoveAsync(entity, token);
    }

    public virtual async ValueTask<IEnumerable<TEntity>> GetAsync(Expression<Func<TEntity, bool>> query, bool @readonly = true, CancellationToken cancellation = default)
    {
        return await DbSet
            .AsNoTracking(@readonly)
            .Where(query)
            .ToListAsync(cancellation);
    }

    public virtual async Task<IEnumerable<TEntity>> GetAllAsync(bool @readonly = true, CancellationToken cancellation = default)
    {
        return await DbSet
            .AsNoTracking(@readonly)
            .ToListAsync(cancellation);
    }

    public virtual async ValueTask<TEntity?> FindAsync(object id, CancellationToken cancellation = default)
    {
        return await DbSet.FindAsync(new object?[] { id }, cancellationToken: cancellation);
    }

    public virtual async ValueTask<bool> AnyAsync(Expression<Func<TEntity, bool>>? expression = null, CancellationToken cancellation = default)
    {
        if (cancellation.IsCancellationRequested)
            return false;

        return expression is null
            ? await DbSet.AnyAsync(cancellation)
            : await DbSet.AnyAsync(expression, cancellation);
    }

    public virtual async ValueTask<int> CountAsync(Expression<Func<TEntity, bool>>? expression = null, CancellationToken cancellation = default)
    {
        if (cancellation.IsCancellationRequested)
            return 0;

        return expression is null
            ? await DbSet.CountAsync(cancellation)
            : await DbSet.CountAsync(expression, cancellation);
    }

    protected virtual async ValueTask<bool> RemoveAsync(TEntity obj, CancellationToken token = default)
    {
        if (token.IsCancellationRequested) return false;

        var entry = DbSet.Remove(obj);

        return await Task.FromResult(entry.State == EntityState.Deleted);
    }

    #endregion CRUD ASYNCHRONOUS

    #region DIPOSABLE IMPLEMENTATION

    public void Dispose()
    {
        GC.SuppressFinalize(this);
    }

    #endregion DIPOSABLE IMPLEMENTATION
}