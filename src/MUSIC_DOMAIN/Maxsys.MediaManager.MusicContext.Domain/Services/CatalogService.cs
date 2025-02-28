using System.Linq.Expressions;
using AutoMapper;
using Maxsys.Core.Interfaces.Data;
using Maxsys.Core.Services;
using Maxsys.MediaManager.MusicContext.Domain.Interfaces.Repositories;
using Maxsys.MediaManager.MusicContext.Domain.Interfaces.Services;

namespace Maxsys.MediaManager.MusicContext.Domain.Services;

public class CatalogService : ServiceBase<Catalog, ICatalogRepository, Guid>, ICatalogService
{
    public CatalogService(ICatalogRepository repository, IUnitOfWork uow, IMapper mapper)
        : base(repository, uow, mapper)
    { }

    protected override Expression<Func<Catalog, bool>> IdSelector(Guid id) => x => x.Id == id;
}