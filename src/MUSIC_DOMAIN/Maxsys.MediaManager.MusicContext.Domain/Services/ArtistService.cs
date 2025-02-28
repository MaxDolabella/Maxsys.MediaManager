using System.Linq.Expressions;
using AutoMapper;
using Maxsys.Core.Interfaces.Data;
using Maxsys.Core.Services;
using Maxsys.MediaManager.MusicContext.Domain.Interfaces.Repositories;
using Maxsys.MediaManager.MusicContext.Domain.Interfaces.Services;

namespace Maxsys.MediaManager.MusicContext.Domain.Services;

public class ArtistService : ServiceBase<Artist, IArtistRepository, Guid>, IArtistService
{
    public ArtistService(IArtistRepository repository, IUnitOfWork uow, IMapper mapper)
        : base(repository, uow, mapper)
    { }

    protected override Expression<Func<Artist, bool>> IdSelector(Guid id) => x => x.Id == id;
}