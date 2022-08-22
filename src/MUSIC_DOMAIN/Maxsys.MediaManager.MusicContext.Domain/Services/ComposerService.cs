using Maxsys.MediaManager.MusicContext.Domain.Interfaces.Repositories;
using Maxsys.MediaManager.MusicContext.Domain.Interfaces.Services;

namespace Maxsys.MediaManager.MusicContext.Domain.Services;

public class ComposerService : IComposerService
{
    private readonly IComposerRepository _repository;

    #region CONTRUCTORS

    public ComposerService(IComposerRepository repository)
    {
        _repository = repository;
    }

    #endregion CONTRUCTORS
}