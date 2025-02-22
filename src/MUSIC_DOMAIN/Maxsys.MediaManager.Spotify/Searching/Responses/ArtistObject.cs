namespace Maxsys.MediaManager.Spotify.Searching.Responses;

public class ArtistObject
{
    /// <summary>
    /// The <see href="https://developer.spotify.com/documentation/web-api/reference/search#:~:text=The%20Spotify%20ID%20for%20the%20artist.">Spotify ID</see> for the artist.
    /// </summary>
    public string Id { get; set; }

    /// <summary>
    /// The name of the artist.
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// The popularity of the artist. The value will be between 0 and 100, with 100
    /// being the most popular. The artist's popularity is calculated from the popularity
    /// of all the artist's tracks.
    /// </summary>
    public int Popularity { get; set; }

    /// <summary>
    /// A list of the genres the artist is associated with. If not yet classified,
    /// the array is empty.
    /// </summary>
    /// <example>["Prog rock", "Grunge"]</example>
    public List<string> Genres { get; set; }

    /// <summary>
    /// Images of the artist in various sizes, widest first.
    /// </summary>
    public List<ImageObject> Images { get; set; }
}