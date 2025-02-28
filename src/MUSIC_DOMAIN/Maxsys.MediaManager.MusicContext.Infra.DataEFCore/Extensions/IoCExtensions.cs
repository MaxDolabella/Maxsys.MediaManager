using Maxsys.Core.Extensions;
using Maxsys.Core.Interfaces.Data;
using Maxsys.MediaManager.MusicContext.Domain.Interfaces.Repositories;
using Maxsys.MediaManager.MusicContext.Infra.DataEFCore.Context;
using Maxsys.MediaManager.MusicContext.Infra.DataEFCore.Context.Interceptors;
using Maxsys.MediaManager.MusicContext.Infra.DataEFCore.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Maxsys.MediaManager.MusicContext.Infra.DataEFCore.Extensions;

public static class IoCExtensions
{
    /// <summary>
    /// Adds dependecy injection from Maxsys.MediaManager.MusicContext.Domain
    /// </summary>
    public static IServiceCollection AddDataEFCoreDependencyInjection(this IServiceCollection services, IConfiguration configuration, ServiceLifetime lifetime = ServiceLifetime.Scoped)
    {
        services.ConfigureContext(configuration);
        services.ConfigureUnitOfWork();
        services.ConfigureRepositories();
        return services;
    }

    public static IServiceCollection ConfigureUnitOfWork(this IServiceCollection services, ServiceLifetime lifetime = ServiceLifetime.Scoped)
    {
        services.Add<IUnitOfWork, AppUnitOfWork>(lifetime);

        return services;
    }

    public static IServiceCollection ConfigureRepositories(this IServiceCollection services, ServiceLifetime lifetime = ServiceLifetime.Scoped)
    {
        services.Add<ICatalogRepository, CatalogRepository>(lifetime);
        services.Add<IArtistRepository, ArtistRepository>(lifetime);
        services.Add<IAlbumRepository, AlbumRepository>(lifetime);
        services.Add<IComposerRepository, ComposerRepository>(lifetime);
        services.Add<ISongRepository, SongRepository>(lifetime);
        services.Add<IPlaylistRepository, PlaylistRepository>(lifetime);

        return services;
    }

    /// <summary>
    /// Register database using native Dependency Injection
    /// </summary>
    public static void ConfigureContext(this IServiceCollection services, IConfiguration configuration)
    {
        var conn = configuration.GetConnectionString(nameof(MusicAppContext));

        services.AddScoped<AuditableEntityInterceptor>();

        services.AddDbContext<MusicAppContext>((sp, options) =>
        {
            options.AddInterceptors(sp.GetRequiredService<AuditableEntityInterceptor>());
            options.UseSqlServer(conn);
        });
    }
}