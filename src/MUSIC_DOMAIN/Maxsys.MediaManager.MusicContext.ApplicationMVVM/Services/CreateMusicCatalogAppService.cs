using FluentValidation.Results;
using Maxsys.AppCore;
using Maxsys.DataCore.Interfaces;
using Maxsys.MediaManager.MusicContext.ApplicationMVVM.Interfaces.Services;
using Maxsys.MediaManager.MusicContext.ApplicationMVVM.Models;
using Maxsys.MediaManager.MusicContext.Domain.DTO;
using Maxsys.MediaManager.MusicContext.Domain.Factories;
using Maxsys.MediaManager.MusicContext.Domain.Interfaces.Repositories;
using Maxsys.MediaManager.MusicContext.Domain.Interfaces.Services;
using Maxsys.MediaManager.MusicContext.Domain.Validators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Maxsys.MediaManager.MusicContext.ApplicationMVVM.Services
{
    public class CreateMusicCatalogAppService : ApplicationServiceBase, ICreateMusicCatalogAppService
    {
        private readonly IMusicCatalogService _service;
        private readonly IMusicCatalogRepository _repository;

        public CreateMusicCatalogAppService(
            IUnitOfWork uow,
            IMusicCatalogService service,
            IMusicCatalogRepository repository)
            : base(uow)
        {
            _service = service;
            _repository = repository;
        }

        public async Task<ValidationResult> AddNewMusicCatalogAsync(CreateMusicCatalogModel model)
        {
            BeginTransaction();

            var entity = MusicCatalogFactory.Create(model.Id, model.Name);
            var validator = new MusicCatalogValidator(_repository)
                .SetRulesForCreation();

            var validationResult = await _service.AddAsync(entity, validator);

            return validationResult.IsValid
                ? await CommitAsync()
                : await Task.FromResult(validationResult);
        }

        public async Task<IReadOnlyList<MusicCatalogInfoModel>> GetMusicCatalogsAsync()
        {
            var dtos = await _repository.GetMusicCatalogListAsync();

            return dtos.Select(dto => new MusicCatalogInfoModel(dto)).ToList();
        }

        #region DIPOSABLE IMPLEMENTATION

        public new void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion DIPOSABLE IMPLEMENTATION
    }
}