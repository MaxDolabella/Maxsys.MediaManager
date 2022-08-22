using Maxsys.ModelCore;

namespace Maxsys.MediaManager.MusicContext.Domain.Entities;

[System.Diagnostics.DebuggerDisplay("{Name}")]
public class Catalog : EntityBase
{
    #region PROPERTIES

    public string Name { get; protected set; }

    // Collections
    public virtual ICollection<Artist> Artists { get; protected set; }

    #endregion PROPERTIES

    #region CONSTRUCTORS

    protected Catalog()
    {
        Artists = new List<Artist>();
    }

    internal Catalog(Guid id, string name) : this()
    {
        Id = id;
        Name = name;
    }

    #endregion CONSTRUCTORS
}