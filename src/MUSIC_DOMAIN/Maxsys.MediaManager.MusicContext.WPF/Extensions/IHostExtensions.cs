using Maxsys.MediaManager.MusicContext.Infra.DataEFCore.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Maxsys.MediaManager.MusicContext.WPF.Extensions
{
    public static class IHostExtensions
    {
        public static async Task<IHost> CreateDatabaseIfNotExistAndApplyMigrationsAsync(this IHost host /*, bool seed = false*/)
        {
            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var logger = services.GetRequiredService<ILogger<App>>();
                try
                {
                    var context = services.GetRequiredService<MusicAppContext>();

                    if ((await context.Database.GetPendingMigrationsAsync()).Any())
                    {
                        logger.LogInformation("Applying database migrations.");
                        await context.Database.MigrateAsync();
                        logger.LogInformation("Database migrations applied.");
                    }
                    else
                    {
                        logger.LogInformation("Creating database if not exist.");
                        var created = await context.Database.EnsureCreatedAsync();
                        logger.LogInformation(created ? "Database created!" : "Database not created. Maybe it already exists.");
                    }

                    /* Seed data
                    if (seed)
                    {
                        logger.LogInformation("Seeding database.");
                        await context.SeedAsync();
                        logger.LogInformation("Seeding database finished.");
                    }
                    //*/
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, $"Error at database creation or migrations: ");
                }
            }
            return host;
        }
    }
}