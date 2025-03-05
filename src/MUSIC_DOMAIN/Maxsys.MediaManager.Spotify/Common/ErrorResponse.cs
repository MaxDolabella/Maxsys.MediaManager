namespace Maxsys.MediaManager.Spotify.Common;

public class ErrorResponse
{
    /// <summary>
    /// The HTTP status code (also returned in the response header; see <see href="https://developer.spotify.com/documentation/web-api/reference/search#:~:text=The%20HTTP%20status%20code%20(also%20returned%20in%20the%20response%20header%3B%20see%20Response%20Status%20Codes%20for%20more%20information).">Response Status Codes</see> for more information).
    /// </summary>
    public int Status { get; set; }

    /// <summary>
    /// A short description of the cause of the error.
    /// </summary>
    public string Message { get; set; }
}