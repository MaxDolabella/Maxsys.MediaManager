using System.Diagnostics;
using Maxsys.Core.Entities;
using Maxsys.MediaManager.MusicContext.Domain.Enums;

namespace Maxsys.MediaManager.MusicContext.Domain.Entities;

[DebuggerDisplay("[{Id.ToString().Substring(0,4)}] {Name}")]
public class Album : Entity<Guid>
{
    #region PROPERTIES

    public Guid ArtistId { get; protected set; }

    /// <summary>
    /// Path limit is 248 characters.
    /// Path+Filename limit is 260 characters.
    /// </summary>
    public Uri Directory { get; protected set; }

    public string Name { get; protected set; }
    public short? Year { get; protected set; }
    public string Genre { get; protected set; }
    public AlbumTypes AlbumType { get; protected set; }

    // TODO criar uma classe pra não repetir no banco
    public byte[] AlbumCover { get; protected set; }
    public string? SpotifyID { get; protected set; }

    #region Navigation

    public Artist Artist { get; protected set; }

    public List<Song> Songs { get; protected set; } = [];

    #endregion Navigation

    #endregion PROPERTIES

    public string GetYearName()
        => $"{(Year.HasValue ? $"({Year.Value}) " : string.Empty)}{Name}";

    #region CONSTRUCTORS

    protected Album()
    { }

    internal Album(Guid id, Guid artistId, Uri directory, string name, short? year, string genre, byte[] albumCover, AlbumTypes albumType, string? spotifyRef) : this()
    {
        (Id, ArtistId, Directory, Name, Year, Genre, AlbumCover, AlbumType, SpotifyID) =
        (id, artistId, directory, name, year, genre, albumCover, albumType, spotifyRef);
    }

    #endregion CONSTRUCTORS
}