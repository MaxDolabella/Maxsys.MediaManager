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
    public sealed class DeleteArtistAppService
        : ApplicationServiceBase, IDeleteArtistAppService
    {
        private readonly ILogger _logger;
        private readonly IArtistService _service;
        private readonly IArtistRepository _repository;

        public DeleteArtistAppService(IUnitOfWork uow,
            IArtistService service,
            IArtistRepository repository,
            ILogger<DeleteArtistAppService> logger)
            : base(uow)
        {
            _logger = logger;
            _service = service;
            _repository = repository;
        }

        public async Task<ValidationResult> DeleteArtistAsync(ArtistListModel model)
        {
            BeginTransaction();

            var entityForDeletion = await _repository.GetByIdAsync(model.ArtistId, @readonly: false);

            // Validate Entity
            var validator = new ArtistValidator(_repository).SetRulesForDeletion();
            var validationResult = await validator.ValidateAsync(entityForDeletion);

            if (!validationResult.IsValid)
                return validationResult;

            // Deletion
            validationResult = await _service.RemoveAsync(entityForDeletion.Id);

            return validationResult.IsValid
                ? await CommitAsync()
                : validationResult;
        }

        public async Task<IReadOnlyList<ArtistListModel>> GetArtistsAsync()
        {
            var dtos = await _repository.GetArtistInfosAsync();

            return dtos.Select(dto => new ArtistListModel(dto)).ToList();
        }
    }
}