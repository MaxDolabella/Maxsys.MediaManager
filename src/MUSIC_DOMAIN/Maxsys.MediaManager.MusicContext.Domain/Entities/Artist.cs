using Maxsys.Core.Entities;

namespace Maxsys.MediaManager.MusicContext.Domain.Entities;

[System.Diagnostics.DebuggerDisplay("{Name}")]
public class Artist : Entity<Guid>
{
    #region PROPERTIES

    public Guid CatalogId { get; protected set; }
    public string Name { get; protected set; }
    public string? SpotifyId { get; protected set; }

    // Navigation

    public Catalog Catalog { get; protected set; }

    // Collections
    public List<Album> Albums { get; protected set; } = [];

    #endregion PROPERTIES

    public void SetSpotifyId(string? id)
    {
        SpotifyId = id;
    }

    #region CONSTRUCTORS

    protected Artist()
    { }

    internal Artist(Guid id, Guid catalogId, string name)
    {
        Id = id;
        CatalogId = catalogId;

        Name = name;
    }

    #endregion CONSTRUCTORS
}