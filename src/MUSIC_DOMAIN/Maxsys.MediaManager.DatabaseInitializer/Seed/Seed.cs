using System.Reflection;
using System.Text.Json;
using Maxsys.MediaManager.DatabaseInitializer.Extensions;
using Maxsys.MediaManager.MusicContext.Domain.Entities;
using Maxsys.MediaManager.MusicContext.Domain.Factories;
using Microsoft.EntityFrameworkCore;

namespace Maxsys.MediaManager.DatabaseInitializer;

internal static class Seed
{
    private static Assembly Assembly { get; } = Assembly.GetExecutingAssembly();

    public static async Task AsyncSeeding(DbContext context, bool _, CancellationToken cancellationToken)
    {
        if (!await context.Set<Catalog>().AnyAsync(cancellationToken))
        {
            // Catalog
            var catalogs = GetJsonItems<CatalogJson, Catalog>("Catalog",
                x => CatalogFactory.Create(x.Id, x.Name));

            await context.Set<Catalog>().AddRangeAsync(catalogs, cancellationToken);

            await context.SaveChangesAsync(cancellationToken);
        }
    }

    private static IEnumerable<TEntity> GetJsonItems<TJsonModel, TEntity>(string jsonFile, Func<TJsonModel, TEntity> convertion)
    {
        var jsonItems = JsonSerializer.Deserialize<TJsonModel[]>(Assembly.GetSeedJson(jsonFile)!)!;

        return jsonItems.Select(convertion);
    }
}