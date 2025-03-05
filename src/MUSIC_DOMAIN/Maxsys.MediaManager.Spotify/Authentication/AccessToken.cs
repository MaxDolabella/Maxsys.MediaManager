namespace Maxsys.MediaManager.Spotify.Authentication;

public class AccessToken
{
    public string Value { get; set; }
    public DateTimeOffset ExpiresAt { get; set; }

    public bool IsValid()
    {
        return DateTimeOffset.Now.AddMinutes(-1) < ExpiresAt;
    }
}