using System.Text.Json.Serialization;

namespace Maxsys.MediaManager.Spotify.Searching.Responses;

public class AlbumObject
{
    /// <summary>
    /// The <see href="https://developer.spotify.com/documentation/web-api/reference/search#:~:text=The%20Spotify%20ID%20for%20the%20artist.">Spotify ID</see> for the artist.
    /// </summary>
    public string Id { get; set; }

    /// <summary>
    /// The name of the album. In case of an album takedown, the value may be an empty string.
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// The type of the album.
    /// <br/>
    /// Allowed values: "album", "single", "compilation"
    /// </summary>
    [JsonPropertyName("album_type")]
    public SpotifyAlbumTypes AlbumType { get; set; }

    /// <summary>
    /// The number of tracks in the album.
    /// </summary>
    [JsonPropertyName("total_tracks")]
    public int TotalTracks { get; set; }

    /// <summary>
    /// The date the album was first released.
    /// <br/>
    /// Example: "1981-12"
    /// </summary>
    [JsonPropertyName("release_date")]
    public string ReleasedDate { get; set; }

    /// <summary>
    /// The precision with which release_date value is known.
    /// <br/>
    /// Allowed values: "year", "month", "day"
    /// <br/>
    /// Example: "year"
    /// </summary>
    [JsonPropertyName("release_date_precision")]
    public ReleasedDatePrecisions ReleasedDatePrecision { get; set; }

    /// <summary>
    /// Images of the artist in various sizes, widest first.
    /// </summary>
    public List<ImageObject> Images { get; set; }

    /// <summary>
    /// The artists of the album.
    /// Each artist object includes a link in href to more detailed information about the artist.
    /// </summary>
    public List<SimplifiedArtistObject> Artists { get; set; }
}

public class SimplifiedArtistObject
{
    public string Id { get; set; }
    public string Name { get; set; }
}

public enum SpotifyAlbumTypes
{
    Album, Single, Compilation
}

public enum ReleasedDatePrecisions
{
    Year, Month, Day
}