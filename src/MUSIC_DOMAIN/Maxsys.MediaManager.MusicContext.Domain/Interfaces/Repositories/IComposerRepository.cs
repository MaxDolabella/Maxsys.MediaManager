using Maxsys.Core.Interfaces.Repositories;

namespace Maxsys.MediaManager.MusicContext.Domain.Interfaces.Repositories;

public interface IComposerRepository : IRepository<Composer>
{
    // TODO ComposerDTO?
    Task<Composer?> GetByNameAsync(string composerName, bool @readonly = false, CancellationToken token = default);
}