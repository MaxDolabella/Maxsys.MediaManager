using Maxsys.ModelCore;

namespace Maxsys.MediaManager.MusicContext.Domain.Entities;

[System.Diagnostics.DebuggerDisplay("{Name}")]
public class Composer : EntityBase
{
    #region PROPERTIES

    public string Name { get; protected set; }

    // Collections
    public IEnumerable<Song> Songs { get; protected set; }

    #endregion PROPERTIES

    #region CONSTRUCTORS

    protected Composer()
    { }

    internal Composer(Guid id, string name)
    {
        Id = id;
        Name = name;

        Songs = new List<Song>();
    }

    #endregion CONSTRUCTORS
}