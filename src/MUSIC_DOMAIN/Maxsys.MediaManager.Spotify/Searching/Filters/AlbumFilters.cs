namespace Maxsys.MediaManager.Spotify.Searching.Filters;

public class AlbumFilters : SearchFilterBase
{
    // The artist and year filters can be used while searching albums, artists and tracks.
    // You can filter on a single year or a range (e.g. 1955-1960).

    private string? _artist;

    public string? Artist
    {
        get => _artist;
        set
        {
            _artist = value?.Replace(' ', '+');
        }
    }

    public int? Year { get; set; }
}