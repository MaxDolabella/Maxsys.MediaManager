using Maxsys.ModelCore;

namespace Maxsys.MediaManager.MusicContext.Domain.Entities;

[System.Diagnostics.DebuggerDisplay("{Name}")]
public class Artist : EntityBase
{
    #region PROPERTIES

    public string Name { get; protected set; }

    // Navigation
    public Guid CatalogId { get; protected set; }

    public Catalog Catalog { get; protected set; }

    // Collections
    public IEnumerable<Album> Albums { get; protected set; }

    #endregion PROPERTIES

    #region CONSTRUCTORS

    protected Artist()
    {
        Albums = new List<Album>();
    }

    internal Artist(Guid id, Guid catalogId, string name)
        : this()
    {
        Id = id;
        CatalogId = catalogId;

        Name = name;
    }

    #endregion CONSTRUCTORS

    // TODO apagar
    public void SetCatalog(Catalog newCatalog)
    {
        Catalog = newCatalog;
        CatalogId = newCatalog?.Id ?? default;
    }
}