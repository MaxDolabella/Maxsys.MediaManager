using Maxsys.MediaManager.MusicContext.AppTagLibMono.Services;
using Maxsys.MediaManager.MusicContext.Domain.Interfaces.Mp3;
using Microsoft.Extensions.DependencyInjection;

namespace Maxsys.MediaManager.MusicContext.AppTagLibMono.Extensions;

public static class IoCExtensions
{
    /// <summary>
    /// Adds dependecy injection from Maxsys.MediaManager.MusicContext.Domain
    /// </summary>
    public static IServiceCollection AddTagLibMonoDependencyInjection(this IServiceCollection services, ServiceLifetime lifetime = ServiceLifetime.Scoped)
    {
        services.ConfigureTagLibServices(lifetime);

        return services;
    }

    public static IServiceCollection ConfigureTagLibServices(this IServiceCollection services, ServiceLifetime lifetime = ServiceLifetime.Scoped)
    {
        services.Add<ISongPropertiesReader, TagLibMusicPropertiesReader>(lifetime);
        services.Add<ITagService, TagLibService>(lifetime);

        return services;
    }
}