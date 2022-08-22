namespace Maxsys.MediaManager.MusicContext.Infra.DataEFCore.Context;

public static class SeedDataExtensions
{
    public static async Task<bool> SeedDatabaseAsync(this MusicAppContext dbContext)
    {
        var result = false;
        if (!await dbContext.Set<Catalog>().AnyAsync())
        {
            //await dbContext.Set<Catalog>().AddRangeAsync(musicCatalogs);
            //await dbContext.SaveChangesAsync();

            result = true;
        }

        return result;
    }
}