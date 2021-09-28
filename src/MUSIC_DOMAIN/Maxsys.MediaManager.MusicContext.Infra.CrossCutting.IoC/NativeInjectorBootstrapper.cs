using Maxsys.DataCore.Interfaces;
using Maxsys.MediaManager.CoreDomain.Interfaces.Services;
using Maxsys.MediaManager.CoreDomain.Services;
using TagLibMono = Maxsys.MediaManager.MusicContext.AppTagLibMono.Services;
using Maxsys.MediaManager.MusicContext.Domain.Interfaces.Mp3;
using Maxsys.MediaManager.MusicContext.Domain.Interfaces.Repositories;
using Maxsys.MediaManager.MusicContext.Domain.Interfaces.Services;
using Maxsys.MediaManager.MusicContext.Domain.Interfaces.Services.Mp3;
using Maxsys.MediaManager.MusicContext.Domain.Services;
using Maxsys.MediaManager.MusicContext.Infra.DataEFCore;
using Maxsys.MediaManager.MusicContext.Infra.DataEFCore.Context;
using Maxsys.MediaManager.MusicContext.Infra.DataEFCore.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Maxsys.MediaManager.MusicContext.Domain.Options;

namespace Maxsys.MediaManager.MusicContext.Infra.CrossCutting.IoC
{
    public static class NativeInjectorBootstrapper
    {
        /// <summary>
        /// Register services using native Dependency Injection
        /// </summary>
        public static void RegisterServices(this IServiceCollection services)
        {
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            // Repositories
            services.AddScoped<IMusicCatalogRepository, MusicCatalogRepository>();
            services.AddScoped<IArtistRepository, ArtistRepository>();
            services.AddScoped<IAlbumRepository, AlbumRepository>();
            services.AddScoped<IComposerRepository, ComposerRepository>();
            services.AddScoped<IMusicRepository, MusicRepository>();
            services.AddScoped<IPlaylistRepository, PlaylistRepository>();
            services.AddScoped<IMusicRankRepository, MusicRankRepository>();

            // Services
            services.AddScoped<IPathService, PathService>();
            services.AddScoped<IFilePropertiesReader, FilePropertiesReader>();

            // TagLibMono
            services.AddScoped<IMusicPropertiesReader, TagLibMono.TagLibMusicPropertiesReader>();
            services.AddScoped<ITagService, TagLibMono.TagLibService>();

            services.AddScoped<IMusicCatalogService, MusicCatalogService>();
            services.AddScoped<IArtistService, ArtistService>();
            services.AddScoped<IAlbumService, AlbumService>();
            services.AddScoped<IComposerService, ComposerService>();
            services.AddScoped<IMusicService, MusicService>();
            services.AddScoped<IPlaylistService, PlaylistService>();

            //services.AddScoped<IMusicCatalogService, MusicCatalogService>(sp => MusicCatalogServiceFactory.Create(sp.GetService<IMusicCatalogRepository>()));
            //services.AddScoped<IArtistService, ArtistService>(sp => ArtistServiceFactory.Create(sp.GetService<IArtistRepository>()));
            //services.AddScoped<IAlbumService, AlbumService>(sp => AlbumServiceFactory.Create(sp.GetService<IAlbumRepository>()));
            //services.AddScoped<IComposerService, ComposerService>(sp => ComposerServiceFactory.Create(sp.GetService<IComposerRepository>()));
            //services.AddScoped<IMusicService, MusicService>(sp => MusicServiceFactory.Create(sp.GetService<IMusicRepository>()));
            //services.AddScoped<IPlaylistService, PlaylistService>(sp => PlaylistServiceFactory.Create(sp.GetService<IPlaylistRepository>(), sp.GetService<ITagService>()));
        }

        /// <summary>
        /// Register database using native Dependency Injection
        /// </summary>
        public static void RegisterDatabase(this IServiceCollection services, IConfiguration configuration)
        {
            var baseConn = configuration.GetConnectionString(nameof(MusicAppContext));
            /* Connection With Password
            var passConn = new Microsoft.Data.SqlClient.SqlConnectionStringBuilder(baseConn)
            {
                UserID = configuration["DATABASE_AUTH:USER"],
                Password = configuration["DATABASE_AUTH:PASS"]
            }.ToString();
            //*/

            services.AddDbContext<MusicAppContext>(options => options.UseSqlServer(baseConn));
        }

        /// <summary>
        /// Register Logging using native Dependency Injection
        /// </summary>
        public static void RegisterLogging(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddLogging(configure => configure

                .ClearProviders()
                .AddConfiguration(configuration.GetSection("Logging"))
                .AddConsole()

                //.SetMinimumLevel(LogLevel.Trace)

                /* ConsoleLogger example
                .AddColorConsoleLogger(configuration =>
                {
                    configuration.LogLevel = LogLevel.Warning;
                    configuration.Color = ConsoleColor.DarkMagenta;
                })
                //*/// ================================================

                /* FileLogger example
                .AddFileLogger(options =>
                {
                    options.MaxFileSizeInMB = 5;
                    options.RetainPolicyFileCount = 10;
                    options.FileNamePrefix = "InfoLog";
                    options.LogLevel = LogLevel.Information;
                })

                .AddFileLogger(options =>
                {
                    options.MaxFileSizeInMB = 5;
                    options.RetainPolicyFileCount = 2;
                    options.FileNamePrefix = "TraceLog";
                    options.LogLevel = LogLevel.Debug;
                })

                //*/// ================================================
                );
        }

        /// <summary>
        /// Adds <see cref="MusicSettings"/> options using native Dependency Injection.
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        public static void RegisterOptions(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddOptions();
            services.Configure<MusicSettings>(configuration.GetSection(MusicSettings.Section));
        }
    }
}