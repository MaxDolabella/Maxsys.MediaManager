using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace Microsoft.EntityFrameworkCore
{
    internal static class EFCoreExtensions
    {
        /// <summary>
        /// Extension to make <see cref="EntityFrameworkQueryableExtensions.AsNoTracking{TEntity}(IQueryable{TEntity})">AsNoTracking()</see>
        /// more readable for the repository class.
        /// <para/>
        /// The change tracker will not track any of the entities that are returned from
        /// a LINQ query. If the entity instances are modified, this will not be detected
        /// by the change tracker and <see cref="DbContext.SaveChanges()"/>
        /// will not persist those changes to the database.
        /// <para/>
        /// Disabling change tracking is useful for read-only scenarios because it avoids
        /// the overhead of setting up change tracking for each entity instance. You should
        /// not disable change tracking if you want to manipulate entity instances and persist
        /// those changes to the database using <see cref="DbContext.SaveChanges()"/>.
        /// <para/>
        /// Identity resolution will not be performed. If an entity with a given key is in
        /// different result in the result set then they will be different instances.
        /// <para/>
        /// The default tracking behavior for queries can be controlled by
        /// <see cref="ChangeTracking.ChangeTracker"/>.QueryTrackingBehavior.
        /// </summary>
        /// <typeparam name="TEntity">The type of entity being queried.</typeparam>
        /// <param name="source">The source query.</param>
        /// <param name="track">Indicates wether the result set will be tracked by the context.
        /// If <see langword="true"/>, will be tracked. Otherwise, will not be tracked.</param>
        /// <exception cref="System.ArgumentNullException">source is null.</exception>
        /// <returns>A new query where the result set will or will not be tracked by the context, depending on the <paramref name="track"/> parameter.</returns>
        public static IQueryable<TEntity> AsNoTracking<TEntity>(
            [NotNullAttribute] this IQueryable<TEntity> source,
            bool track) where TEntity : class
        {
            return track ? source : source.AsNoTracking();
        }
    }
}