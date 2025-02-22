namespace Maxsys.MediaManager.Spotify.Common;

public class ImageObject
{
    /// <summary>
    /// The source URL of the image.
    /// </summary>
    public Uri Url { get; set; }

    /// <summary>
    /// The image height in pixels.
    /// </summary>
    public int? Height { get; set; }

    /// <summary>
    /// The image width in pixels.
    /// </summary>
    public int? Width { get; set; }
}