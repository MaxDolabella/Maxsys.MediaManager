using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation.Results;
using Maxsys.Core.Interfaces.Data;
using Maxsys.Core.Services;
using Maxsys.MediaManager.MusicContext.ApplicationMVVM.Interfaces.Services;
using Maxsys.MediaManager.MusicContext.ApplicationMVVM.Interfaces.ViewModels;
using Maxsys.MediaManager.MusicContext.ApplicationMVVM.ViewModels.Catalog;
using Maxsys.MediaManager.MusicContext.Domain.Interfaces.Repositories;
using Maxsys.MediaManager.MusicContext.Domain.Interfaces.Services;

namespace Maxsys.MediaManager.MusicContext.ApplicationMVVM.Services
{
    public class CatalogCreateAppService : ServiceBase, ICatalogCreateAppService
    {
        private readonly IUnitOfWork _uow;
        private readonly ICatalogService _service;
        private readonly ICatalogRepository _repository;

        public CatalogCreateAppService(
            IUnitOfWork uow,
            ICatalogService service,
            ICatalogRepository repository)
        {
            _uow = uow;
            _service = service;
            _repository = repository;
        }

        public async Task<ValidationResult> CreateCatalogAsync(ICatalogCreateViewModel model)
        {
            throw new NotImplementedException();
            //await _uow.BeginTransactionAsync();

            //var entity = CatalogFactory.Create(model.Name);
            //var validator = new CatalogValidator(_repository)
            //    .SetRulesForCreation();

            //var validationResult = await _service.AddAsync(entity, validator);

            //return validationResult.IsValid
            //    ? await _uow.CommitAsync()
            //    : await Task.FromResult(validationResult);
        }

        public async Task<IReadOnlyList<CatalogDetailViewModel>> GetMusicCatalogsAsync()
        {
            var dtos = await _repository.GetCatalogDetailsAsync();

            return dtos.Select(dto => new CatalogDetailViewModel(dto)).ToList();
        }
    }
}