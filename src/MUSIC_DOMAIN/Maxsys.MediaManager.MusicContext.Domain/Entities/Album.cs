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
    public SysPath Directory { get; protected set; }

    public string Name { get; protected set; }
    public short? Year { get; protected set; }
    public string Genre { get; protected set; }
    public AlbumTypes Type { get; protected set; }

    // TODO criar uma classe pra não repetir no banco
    public byte[] Cover { get; protected set; }

    public SpotifyID? SpotifyID { get; protected set; }

    // Navigation

    public Artist Artist { get; protected set; }
    public List<Song> Songs { get; protected set; } = [];

    #endregion PROPERTIES

    public string GetYearName()
        => $"{(Year.HasValue ? $"({Year.Value}) " : string.Empty)}{Name}";

    public void SetSpotifyID(string? id)
    {
        if (SpotifyID != id)
        {
            SpotifyID = id;
        }
    }


    public void SetCover(byte[] newCover)
    {
        if (newCover?.Any() != true)
        {
            throw new Maxsys.Core.Exceptions.DomainException("'Cover' cannot be null or empty.");
        }

        if (Cover != newCover)
        {
            Cover = newCover;
        }
    }

    #region CONSTRUCTORS

    protected Album()
    { }

    internal Album(Guid id, Guid artistId, string directory, string name, short? year, string genre, byte[] albumCover, AlbumTypes albumType, string? spotifyID) : this()
    {
        (Id, ArtistId, Directory, Name, Year, Genre, Cover, Type, SpotifyID) =
        (id, artistId, new SysPath(directory), name, year, genre, albumCover, albumType, (SpotifyID?)spotifyID);
    }

    #endregion CONSTRUCTORS
}