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
    public sealed class DeleteAlbumAppService
        : ApplicationServiceBase, IDeleteAlbumAppService
    {
        private readonly ILogger _logger;
        private readonly IAlbumService _service;
        private readonly IAlbumRepository _albumRepository;

        public DeleteAlbumAppService(IUnitOfWork uow,
            IAlbumService service,
            IAlbumRepository albumRepository,
            ILogger<CreateAlbumAppService> logger)
            : base(uow)
        {
            _service = service;
            _albumRepository = albumRepository;
            _logger = logger;
        }

        public async Task<ValidationResult> DeleteAlbumAsync(AlbumListModel model)
        {
            
            BeginTransaction();

            var entityForDeletion = await _albumRepository.GetByIdAsync(model.AlbumId, @readonly: false);

            // Validate Entity
            var validator = new AlbumValidator(_albumRepository).SetRulesForDeletion();
            var validationResult = await validator.ValidateAsync(entityForDeletion);

            if (!validationResult.IsValid)
                return validationResult;

            // Deletion
            validationResult = await _service.RemoveAsync(entityForDeletion.Id);

            return validationResult.IsValid
                ? await CommitAsync()
                : validationResult;
        }

        public async Task<IReadOnlyList<AlbumListModel>> GetAlbumsAsync()
        {
            var dtos = await _albumRepository.GetAlbumDetailsAsync();

            return dtos.Select(dto => new AlbumListModel(dto)).ToList();
        }
    }
}