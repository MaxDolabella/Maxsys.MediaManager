using Maxsys.MediaManager.MusicContext.Domain.Entities;
using Maxsys.MediaManager.MusicContext.Domain.Interfaces.Repositories;
using Maxsys.MediaManager.MusicContext.Domain.Interfaces.Services;
using Maxsys.ModelCore.Services;

namespace Maxsys.MediaManager.MusicContext.Domain.Services
{
    public class ComposerService : ServiceBase<Composer>, IComposerService
    {
        private readonly IComposerRepository _repository;

        #region CONTRUCTORS

        public ComposerService(IComposerRepository repository) : base(repository)
        {
            _repository = repository;
        }

        #endregion CONTRUCTORS
    }
}