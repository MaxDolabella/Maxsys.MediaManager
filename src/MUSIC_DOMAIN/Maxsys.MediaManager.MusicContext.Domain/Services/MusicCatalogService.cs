using FluentValidation.Results;
using Maxsys.MediaManager.MusicContext.Domain.Entities;
using Maxsys.MediaManager.MusicContext.Domain.Interfaces.Repositories;
using Maxsys.MediaManager.MusicContext.Domain.Interfaces.Services;
using Maxsys.MediaManager.MusicContext.Domain.Validators;
using Maxsys.ModelCore.Services;
using System.Threading.Tasks;

namespace Maxsys.MediaManager.MusicContext.Domain.Services
{
    public class MusicCatalogService : ServiceBase<MusicCatalog>, IMusicCatalogService
    {
        private readonly IMusicCatalogRepository _repository;

        #region CONTRUCTORS

        public MusicCatalogService(IMusicCatalogRepository repository) : base(repository)
        {
            _repository = repository;
        }

        #endregion CONTRUCTORS
    }
}