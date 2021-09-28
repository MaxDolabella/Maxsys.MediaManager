using Maxsys.ModelCore;
using Maxsys.ModelCore.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Maxsys.MediaManager.MusicContext.Infra.DataEFCore.Repositories.Common
{
    // https://github.com/Arch/UnitOfWork/blob/master/src/UnitOfWork/Repository.cs
    /// <inheritdoc/>
    public abstract class RepositoryBase<TEntity> : IRepositoryBase<TEntity>
        where TEntity : EntityBase
    {
        #region FIELDS

        protected readonly DbContext Context;
        protected readonly DbSet<TEntity> DbSet;
        protected IQueryable<TEntity> ReadOnlySet => DbSet.AsNoTracking();
        #endregion FIELDS

        #region CONSTRUCTOR

        public RepositoryBase(DbContext dbContext)
        {
            Context = dbContext;
            DbSet = Context.Set<TEntity>();
        }

        #endregion CONSTRUCTOR

        #region CRUD SYNCHRONOUS

        /// <inheritdoc/>
        public virtual bool Add(TEntity obj)
        {
            var entry = DbSet.Add(obj);

            return entry.State == EntityState.Added;
        }

        /// <inheritdoc/>
        public virtual bool Update(TEntity obj)
        {
            var entry = DbSet.Update(obj);

            return entry.State == EntityState.Modified;
        }

        /// <inheritdoc/>
        public virtual bool Remove(TEntity obj)
        {
            var tracker = DbSet.Remove(obj);

            return tracker.State == EntityState.Detached
                || tracker.State == EntityState.Deleted;
        }

        /// <inheritdoc/>
        public virtual bool Remove(object id)
        {
            var entity = DbSet.Find(id);

            return entity is null || Remove(entity);
        }

        /// <inheritdoc/>
        public virtual IEnumerable<TEntity> GetAll(bool @readonly = false)
        {
            return DbSet.AsNoTracking(@readonly)
                .AsEnumerable();
        }

        /// <inheritdoc/>
        public virtual IEnumerable<TEntity> Find(Expression<Func<TEntity, bool>> predicate
            , bool @readonly)
        {
            return DbSet.AsNoTracking(@readonly)
                .Where(predicate)
                .AsEnumerable();
        }

        #endregion CRUD SYNCHRONOUS

        #region CRUD ASYNCHRONOUS

        /// <inheritdoc/>
        public virtual async Task<bool> AddAsync(TEntity obj)
        {
            var entry = await DbSet.AddAsync(obj);

            return entry.State == EntityState.Added;
        }

        /// <inheritdoc/>
        public virtual async Task<bool> UpdateAsync(TEntity obj)
        {
            return await Task.Run(() =>
            {
                return Update(obj);
            });
        }

        /// <inheritdoc/>
        public virtual async Task<bool> RemoveAsync(object id)
        {
            var entity = await DbSet.FindAsync(id);

            return entity is null || await RemoveAsync(entity);
        }

        /// <inheritdoc/>
        public virtual async Task<bool> RemoveAsync(TEntity obj)
        {
            return await Task.Run(() =>
            {
                return Remove(obj);
            });
        }

        /*{
            IQueryable<TEntity> query = @readonly
                ? DbSet.AsNoTracking()
                : DbSet;

            return await query.SingleOrDefaultAsync(obj => obj.Id == (Guid)id);
            //return await DbSet.FindAsync(id);
        }*/

        /// <inheritdoc/>
        public virtual async Task<IEnumerable<TEntity>> GetAllAsync(bool @readonly = true)
        {
            var query = DbSet.AsNoTracking(@readonly);

            return await query.ToListAsync();
        }


        //public abstract TEntity GetById(object key, bool @readonly = true);
        public virtual TEntity GetById(object key, bool @readonly = true) => DbSet.AsNoTracking(@readonly).FirstOrDefault(obj => obj.Id == (Guid)key);

        //public abstract Task<TEntity> GetByIdAsync(object key, bool @readonly = true);
        public virtual async Task<TEntity> GetByIdAsync(object key, bool @readonly = true) => await DbSet.AsNoTracking(@readonly).FirstOrDefaultAsync(obj => obj.Id == (Guid)key);
        

        /// <inheritdoc/>
        public virtual async Task<IEnumerable<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate, bool @readonly = true)
        {
            var query = DbSet.AsNoTracking(@readonly)
                .Where(predicate);

            return await query.ToListAsync();
        }

        #endregion CRUD ASYNCHRONOUS

        #region DIPOSABLE IMPLEMENTATION

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        #endregion DIPOSABLE IMPLEMENTATION
    }
}