namespace Maxsys.MediaManager.MusicContext.Domain.Entities;

[System.Diagnostics.DebuggerDisplay("{Id.ToString().Substring(0, 4)} - {Title}")]
public class Song : MediaFile
{
    #region PROPERTIES

    public string Title { get; protected set; }
    public int? TrackNumber { get; protected set; }
    public string? Lyrics { get; protected set; }
    public string? Comments { get; protected set; }

    // Value Objects
    public Classification Classification { get; protected set; }

    public SongDetails SongDetails { get; protected set; }
    public SongProperties SongProperties { get; protected set; }

    // Navigation
    public Guid AlbumId { get; protected set; }

    public Album Album { get; protected set; }

    // Collections
    public ICollection<Composer> Composers { get; protected set; }

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
        SongDetails musicDetails,
        Classification classification,
        SongProperties musicProperties)
        : base(id, fullPath, originalFileName, fileSize)
    {
        AlbumId = albumId;

        Title = title;
        TrackNumber = trackNumber;
        Lyrics = lyrics;
        Comments = comments;

        SongDetails = musicDetails;
        Classification = classification;
        SongProperties = musicProperties;

        Composers = new List<Composer>();
    }

    #endregion CONSTRUCTORS

    #region METHODS

    public void AddComposer(Composer composer)
    {
        Composers ??= new List<Composer>();

        if (!Composers.Any(x => x.Id == composer.Id))
            Composers.Add(composer);
    }

    public void UpdateSongProperties(SongProperties newSongProperties)
    {
        SongProperties.Update(newSongProperties);
    }

    public void UpdateAlbum(Guid newAlbumId, string newFullPath, int? trackNumber)
    {
        AlbumId = newAlbumId;
        FullPath = newFullPath;
        TrackNumber = trackNumber;
    }

    #endregion METHODS
}