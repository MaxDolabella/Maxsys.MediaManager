using System.Text.Json.Serialization;

namespace Maxsys.MediaManager.Spotify.Authentication;

public class AuthenticationResponse
{
    [JsonPropertyName("access_token")]
    public string Token { get; set; }

    public DateTimeOffset ExpiresAt { get; } = DateTimeOffset.Now.AddSeconds(3600);
}