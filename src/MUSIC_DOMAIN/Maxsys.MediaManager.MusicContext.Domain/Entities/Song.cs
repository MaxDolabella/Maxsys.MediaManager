using System.Diagnostics;
using Maxsys.Core.Exceptions;

namespace Maxsys.MediaManager.MusicContext.Domain.Entities;

[DebuggerDisplay($"{{GetDebuggerDisplay(),nq}}")]
public class Song : MediaFile
{
    #region PROPERTIES

    public Guid AlbumId { get; protected set; }

    public string Title { get; protected set; }
    public int? TrackNumber { get; protected set; }
    public string? Lyrics { get; protected set; }
    public string? Comments { get; protected set; }
    public string? SpotifyID { get; protected set; }
    public string? ISRC { get; protected set; }

    // Value Objects
    public Classification Classification { get; protected set; }
    public SongDetails SongDetails { get; protected set; }
    public SongProperties SongProperties { get; protected set; }

    // Navigation

    public Album Album { get; protected set; }

    public List<Composer> Composers { get; protected set; } = [];


    #endregion PROPERTIES

    #region CONSTRUCTORS

    protected Song()
    { }

    internal Song(
        Guid id,
        Guid albumId,
        string originalFileName,
        string fullPath,
        long fileSize,
        string title,
        int? trackNumber,
        string? lyrics,
        string? comments,
        string? spotifyID,
        string? isrc,
        SongDetails musicDetails,
        Classification classification,
        SongProperties musicProperties)
        : base(id, new(fullPath), originalFileName, fileSize)
    {
        AlbumId = albumId;

        Title = title;
        TrackNumber = trackNumber;
        Lyrics = lyrics;
        Comments = comments;
        SpotifyID = spotifyID;
        ISRC = isrc;

        SongDetails = musicDetails;
        Classification = classification;
        SongProperties = musicProperties;
    }

    #endregion CONSTRUCTORS

    #region METHODS

    public void AddComposer(Composer composer)
    {
        if (!Composers.Any(x => x.Id == composer.Id))
            Composers.Add(composer);
    }

    public void UpdateSongProperties(TimeSpan duration, int bitRate)
    {
        SongProperties.Update(duration, bitRate);
    }

    public void UpdateAlbum(Guid albumId, Uri fullPath, int? trackNumber)
    {
        AlbumId = albumId;
        FullPath = fullPath;
        TrackNumber = trackNumber;
    }


    public void SetSpotifyId(string? id)
    {
        SpotifyID = id;
    }

    public void SetISRC(string? isrc)
    {
        isrc = isrc?.Replace("-", string.Empty).Trim().ToUpper();
        if (isrc is not null && isrc.Length != 12)
        {
            throw new DomainException("ISRC must be 12 length alphanumeric.");
        }

        ISRC = isrc;
    }

    private string GetDebuggerDisplay()
    {
        return $"{Id.ToString().Substring(0, 4)} - {Title}";
    }

    #endregion METHODS
}