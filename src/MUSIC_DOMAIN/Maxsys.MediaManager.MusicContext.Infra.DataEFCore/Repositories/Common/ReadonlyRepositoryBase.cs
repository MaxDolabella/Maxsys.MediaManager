using Maxsys.ModelCore.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace Maxsys.MediaManager.MusicContext.Infra.DataEFCore.Repositories.Common
{
    /// <summary>
    /// Provides readonly data access from a repository of type <typeparamref name="TEntity"/>.
    /// </summary>
    /// <typeparam name="TEntity">Type of entity used. Must be a class.</typeparam>
    public abstract class ReadonlyRepositoryBase<TEntity> : IReadonlyRepositoryBase<TEntity> 
        where TEntity : class
    {
        //protected readonly DbSet<TEntity> DbSet;
        protected readonly IQueryable<TEntity> ReadOnlySet;
        
        protected ReadonlyRepositoryBase(DbContext context)
        {
            ReadOnlySet = context.Set<TEntity>().AsNoTracking();
        }

        #region DIPOSABLE IMPLEMENTATION

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        #endregion DIPOSABLE IMPLEMENTATION
    }
}