using System.Diagnostics;
using Maxsys.Core.Entities;

namespace Maxsys.MediaManager.MusicContext.Domain.Entities;

[DebuggerDisplay("{Name}")]
public class Catalog : Entity<Guid>
{
    #region PROPERTIES

    public string Name { get; protected set; }

    public List<Artist> Artists { get; protected set; } = [];

    #endregion PROPERTIES

    #region CONSTRUCTORS

    protected Catalog()
    { }

    internal Catalog(Guid id, string name)
    {
        Id = id;
        Name = name;
    }

    #endregion CONSTRUCTORS
}