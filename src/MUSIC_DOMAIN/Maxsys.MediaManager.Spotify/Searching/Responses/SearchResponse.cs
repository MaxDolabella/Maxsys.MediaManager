namespace Maxsys.MediaManager.Spotify.Searching.Responses;

public class SearchResponse
{
    public ArtistSearchResponse? Artists { get; set; }
    public AlbumSearchResponse? Albums { get; set; }
    public TrackSearchResponse? Tracks { get; set; }
}

public abstract class TypedSearchResponse<TItem>
{
    /// <summary>
    /// The maximum number of items in the response (as set in the query or by default).
    /// </summary>
    public int Limit { get; set; }

    /// <summary>
    /// The offset of the items returned (as set in the query or by default)
    /// </summary>
    public int Offset { get; set; }

    /// <summary>
    /// URL to the previous page of items. ( null if none)
    /// </summary>
    public Uri? Previous { get; set; }

    /// <summary>
    /// URL to the next page of items. ( null if none)
    /// </summary>
    public Uri? Next { get; set; }

    /// <summary>
    /// The total number of items available to return.
    /// </summary>
    public int Total { get; set; }

    public List<TItem> Items { get; set; }
}

public class ArtistSearchResponse : TypedSearchResponse<ArtistObject>
{ }

public class AlbumSearchResponse : TypedSearchResponse<AlbumObject>
{ }

public class TrackSearchResponse : TypedSearchResponse<TrackObject>
{ }