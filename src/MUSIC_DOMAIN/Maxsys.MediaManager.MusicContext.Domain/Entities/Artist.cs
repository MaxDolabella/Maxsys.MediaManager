using System.Diagnostics;
using Maxsys.Core.Entities;

namespace Maxsys.MediaManager.MusicContext.Domain.Entities;

[DebuggerDisplay("{Name}")]
public class Artist : Entity<Guid>
{
    #region PROPERTIES

    public Guid CatalogId { get; protected set; }
    public string Name { get; protected set; }
    public string? SpotifyID { get; protected set; }

    #region Navigation

    public Catalog Catalog { get; protected set; }

    public List<Album> Albums { get; protected set; } = [];

    #endregion Navigation

    #endregion PROPERTIES

    #region CONSTRUCTORS

    protected Artist()
    { }

    internal Artist(Guid id, Guid catalogId, string name, string? spotifyRef)
    {
        Id = id;
        CatalogId = catalogId;

        Name = name;
        SpotifyID = spotifyRef;
    }

    #endregion CONSTRUCTORS
}