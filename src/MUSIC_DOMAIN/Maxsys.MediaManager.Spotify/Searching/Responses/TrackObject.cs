using System.Text.Json.Serialization;

namespace Maxsys.MediaManager.Spotify.Searching.Responses;

public class TrackObject
{
    /// <summary>
    /// The <see href="https://developer.spotify.com/documentation/web-api/reference/search#:~:text=The%20Spotify%20ID%20for%20the%20artist.">Spotify ID</see> for the artist.
    /// </summary>
    public string Id { get; set; }

    /// <summary>
    /// The name of the track.
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// The number of the track. If an album has several discs, the track number is the number on the specified disc.
    /// </summary>
    [JsonPropertyName("track_number")]
    public int TrackNumber { get; set; } = 0;

    /// <summary>
    /// Whether or not the track is from a local file.
    /// </summary>
    [JsonPropertyName("is_local")]
    public bool IsLocal { get; set; }

    /// <summary>
    /// Known external IDs for the track.
    /// </summary>
    [JsonPropertyName("external_ids")]
    public ExternalIdObject? ExternalId { get; set; }

    /// <summary>
    /// The disc number (usually 1 unless the album consists of more than one disc).
    /// </summary>
    [JsonPropertyName("disc_number")]
    public int DiscNumber { get; set; }

    /// <summary>
    /// The track length in milliseconds.
    /// </summary>
    [JsonPropertyName("duration_ms")]
    public int DurationInMs { get; set; }

    ///// <summary>
    ///// The artists who performed the track. Each artist object includes a link in href to more detailed information about the artist.
    ///// </summary>
    //public List<SimplifiedArtistObject> Artists { get; set; }

    /// <summary>
    /// The album on which the track appears. The album object includes a link in href to full information about the album.
    /// </summary>
    public AlbumObject Album { get; set; }
}

public class ExternalIdObject
{
    /// <summary>
    /// International Standard Recording Code
    /// <br/>
    /// <see href="https://en.wikipedia.org/wiki/International_Standard_Recording_Code">Wiki</see>
    /// </summary>
    public string? ISRC { get; set; }
}