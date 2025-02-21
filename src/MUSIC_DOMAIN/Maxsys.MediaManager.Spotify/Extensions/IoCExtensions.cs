using Maxsys.MediaManager.Spotify.Authentication;
using Maxsys.MediaManager.Spotify.Options;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Maxsys.MediaManager.Spotify.Extensions;

public static class IoCExtensions
{
    public static IServiceCollection RegisterServices(this IServiceCollection services, IConfiguration configuration)
    {
        // Options
        services.AddOptions();
        services.Configure<SpotifySettings>(configuration.GetSection(SpotifySettings.SECTION));

        services.AddHttpClient();
        services.AddScoped<ITokenProvider, TokenProvider>();

        return services;
    }
}