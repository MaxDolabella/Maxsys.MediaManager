using FluentValidation.Results;
using Maxsys.AppCore;
using Maxsys.DataCore.Interfaces;
using Maxsys.MediaManager.MusicContext.ApplicationMVVM.Interfaces.Services;
using Maxsys.MediaManager.MusicContext.ApplicationMVVM.Models;
using Maxsys.MediaManager.MusicContext.Domain.Interfaces.Repositories;
using Maxsys.MediaManager.MusicContext.Domain.Interfaces.Services;
using Maxsys.MediaManager.MusicContext.Domain.Validators;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Maxsys.MediaManager.MusicContext.ApplicationMVVM.Services
{
    public sealed class DeleteMusicCatalogAppService
        : ApplicationServiceBase, IDeleteMusicCatalogAppService
    {
        private readonly ILogger _logger;
        private readonly IMusicCatalogService _service;
        private readonly IMusicCatalogRepository _repository;

        public DeleteMusicCatalogAppService(IUnitOfWork uow,
            IMusicCatalogService service,
            IMusicCatalogRepository repository,
            ILogger<DeleteMusicCatalogAppService> logger)
            : base(uow)
        {
            _logger = logger;
            _service = service;
            _repository = repository;
        }

        public async Task<ValidationResult> DeleteMusicCatalogAsync(MusicCatalogListModel model)
        {
            BeginTransaction();

            var entityForDeletion = await _repository.GetByIdAsync(model.MusicCatalogId, @readonly: false);

            // Validate Entity
            var validator = new MusicCatalogValidator(_repository).SetRulesForDeletion();
            var validationResult = await validator.ValidateAsync(entityForDeletion);

            if (!validationResult.IsValid)
                return validationResult;

            // Deletion
            validationResult = await _service.RemoveAsync(entityForDeletion.Id);

            return validationResult.IsValid
                ? await CommitAsync()
                : validationResult;
        }

        public async Task<IReadOnlyList<MusicCatalogListModel>> GetMusicCatalogsAsync()
        {
            var dtos = await _repository.GetMusicCatalogListsAsync();

            return dtos.Select(dto => new MusicCatalogListModel
            {
                MusicCatalogId = dto.MusicCatalogId,
                MusicCatalogName = dto.MusicCatalogName,
                ArtistCount = dto.ArtistCount
            }).ToList();
        }
    }
}