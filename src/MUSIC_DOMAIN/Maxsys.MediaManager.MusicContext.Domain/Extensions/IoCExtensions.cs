using Maxsys.Core.Extensions;
using Maxsys.MediaManager.MusicContext.Domain.Interfaces.Services;
using Maxsys.MediaManager.MusicContext.Domain.Options;
using Maxsys.MediaManager.MusicContext.Domain.Services;
using Maxsys.MediaManager.MusicContext.Domain.Services.PlaylistExporters;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Maxsys.MediaManager.MusicContext.Domain.Extensions;

public static class IoCExtensions
{
    /// <summary>
    /// Adds dependecy injection from Maxsys.MediaManager.MusicContext.Domain
    /// </summary>
    public static IServiceCollection AddDomainDependencyInjection(this IServiceCollection services, IConfiguration configuration, ServiceLifetime lifetime = ServiceLifetime.Scoped)
    {
        services.AddAutoMapper([typeof(IDomainEntry)]);


        services.ConfigureOptions(configuration);

        services.ConfigureServices(lifetime);

        //services.ConfigurePlaylistExporters(lifetime);

        return services;
    }

    /// <summary>
    /// Adiciona a seção <see cref="MusicSettings"/> ao <see cref="IServiceCollection">service provider</see>
    /// </summary>
    public static IServiceCollection ConfigureOptions(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddOptions();
        services.Configure<MusicSettings>(configuration.GetSection(MusicSettings.SECTION));

        return services;
    }

    public static IServiceCollection ConfigureServices(this IServiceCollection services, ServiceLifetime lifetime = ServiceLifetime.Scoped)
    {
        services.Add<IPathService, PathService>(lifetime);

        services.Add<ICatalogService, CatalogService>(lifetime);
        services.Add<IArtistService, ArtistService>(lifetime);
        services.Add<IAlbumService, AlbumService>(lifetime);
        services.Add<IComposerService, ComposerService>(lifetime);
        services.Add<ISongService, SongService>(lifetime);
        services.Add<IPlaylistService, PlaylistService>(lifetime);

        return services;
    }

    public static IServiceCollection ConfigurePlaylistExporters(this IServiceCollection services, ServiceLifetime lifetime = ServiceLifetime.Scoped)
    {
        services.Add<M3UPlaylistFileExporter>(lifetime);
        services.Add<WMPPlaylistFileExporter>(lifetime);

        return services;
    }
}