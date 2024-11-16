using Maxsys.Core.Services;
using Maxsys.MediaManager.MusicContext.Domain.Interfaces.Repositories;
using Maxsys.MediaManager.MusicContext.Domain.Interfaces.Services;

namespace Maxsys.MediaManager.MusicContext.Domain.Services;

public class CatalogService : ServiceBase, ICatalogService
{
    private readonly ICatalogRepository _repository;

    #region CONTRUCTORS

    public CatalogService(ICatalogRepository repository)
    {
        _repository = repository;
    }

    #endregion CONTRUCTORS
}