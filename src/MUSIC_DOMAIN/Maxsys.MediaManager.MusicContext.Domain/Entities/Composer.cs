using System.Diagnostics;
using Maxsys.Core.Entities;

namespace Maxsys.MediaManager.MusicContext.Domain.Entities;

[DebuggerDisplay("{Name}")]
public class Composer : Entity<Guid>
{
    #region PROPERTIES

    public string Name { get; protected set; }

    // Navigation
    public List<Song> Songs { get; protected set; } = [];

    #endregion PROPERTIES

    #region CONSTRUCTORS

    protected Composer()
    { }

    internal Composer(Guid id, string name)
    {
        Id = id;
        Name = name;
    }

    #endregion CONSTRUCTORS
}