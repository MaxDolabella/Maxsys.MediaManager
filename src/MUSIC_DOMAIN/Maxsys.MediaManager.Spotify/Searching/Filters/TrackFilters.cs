namespace Maxsys.MediaManager.Spotify.Searching.Filters;

public class TrackFilters : SearchFilterBase
{
    // The artist and year filters can be used while searching albums, artists and tracks.You can filter on a single year or a range (e.g. 1955-1960).
    // The album filter can be used while searching albums and tracks.
    // The genre filter can be used while searching artists and tracks.
    // The isrc and track filters can be used while searching tracks.

    private string? _artist;
    private string? _album;

    public string? Artist
    {
        get => _artist;
        set
        {
            _artist = value?.Replace(' ', '+');
        }
    }

    public string? Album
    {
        get => _album;
        set
        {
            _album = value?.Replace(' ', '+');
        }
    }

    public string? ISRC { get; set; }

    public int? Year { get; set; }
}