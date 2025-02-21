using Microsoft.Extensions.Configuration;

namespace Maxsys.MediaManager.Spotify.Options;

public class SpotifySettings
{
    public const string SECTION = "spotify";

    [ConfigurationKeyName("auth_url")] public Uri AuthenticationUri { get; set; }
    [ConfigurationKeyName("api_url")] public Uri ApiUri { get; set; }
    [ConfigurationKeyName("spotify_client_id")] public string ClientId { get; set; }
    [ConfigurationKeyName("spotify_client_secret")] public string ClientSecret { get; set; }
    [ConfigurationKeyName("user_id")] public string UserId { get; set; }
}