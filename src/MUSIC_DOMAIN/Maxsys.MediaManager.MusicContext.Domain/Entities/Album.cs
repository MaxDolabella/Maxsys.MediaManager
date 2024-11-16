using Maxsys.Core.Entities;
using Maxsys.MediaManager.MusicContext.Domain.Enums;

namespace Maxsys.MediaManager.MusicContext.Domain.Entities;

[System.Diagnostics.DebuggerDisplay("[{Id.ToString().Substring(0,4)}] {Name}")]
public class Album : Entity<Guid>
{
    #region PROPERTIES

    public Guid ArtistId { get; protected set; }

    /// <summary>
    /// Path limit is 248 characters.
    /// Path+Filename limit is 260 characters.
    /// </summary>
    public string AlbumDirectory { get; protected set; }

    public string Name { get; protected set; }
    public short? Year { get; protected set; }
    public string Genre { get; protected set; }
    public AlbumTypes AlbumType { get; protected set; }

    // TODO criar uma classe pra não repetir no banco
    public byte[] AlbumCover { get; protected set; }

    // Navigation

    public Artist Artist { get; protected set; }

    // Collections
    public List<Song> Songs { get; protected set; } = [];

    #endregion PROPERTIES

    public string GetYearName()
        => $"{(Year.HasValue ? $"({Year.Value}) " : string.Empty)}{Name}";

    #region CONSTRUCTORS

    protected Album()
    { }

    internal Album(Guid id, Guid artistId, string albumDirectory, string name, short? year, string genre, byte[] albumCover, AlbumTypes albumType) : this()
    {
        (Id, ArtistId, AlbumDirectory, Name, Year, Genre, AlbumCover, AlbumType) =
        (id, artistId, albumDirectory, name, year, genre, albumCover, albumType);
    }

    #endregion CONSTRUCTORS
}