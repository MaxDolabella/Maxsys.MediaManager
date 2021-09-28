using Maxsys.MediaManager.MusicContext.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Maxsys.MediaManager.MusicContext.Infra.DataEFCore.Context
{
    public static class SeedDataExtensions
    {
        public static async Task<bool> SeedDatabaseAsync(this MusicAppContext dbContext)
        {
            var result = false;
            if (!await dbContext.Set<MusicCatalog>().AnyAsync())
            {
                //await dbContext.Set<MusicCatalog>().AddRangeAsync(musicCatalogs);
                //await dbContext.SaveChangesAsync();

                result = true;
            }

            return result;
        }
    }
}