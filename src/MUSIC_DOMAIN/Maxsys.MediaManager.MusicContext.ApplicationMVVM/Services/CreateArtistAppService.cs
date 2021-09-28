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
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Maxsys.MediaManager.MusicContext.ApplicationMVVM.Services
{
    public sealed class CreateArtistAppService
        : ApplicationServiceBase, ICreateArtistAppService
    {
        private readonly IArtistService _service;
        private readonly IArtistRepository _artistRepository;
        private readonly IMusicCatalogRepository _musicCatalogRepository;

        public CreateArtistAppService(IUnitOfWork uow,
            IArtistService service,
            IArtistRepository repository,
            IMusicCatalogRepository musicCatalogRepository)
            : base(uow)
        {
            _service = service;
            _artistRepository = repository;
            _musicCatalogRepository = musicCatalogRepository;
        }

        public async Task<ValidationResult> AddNewArtistAsync(CreateArtistModel model)
        {
            BeginTransaction();

            var entity = ArtistFactory.Create(model.Id, model.Name, model.MusicCatalogId);
            var validator = new ArtistValidator(_artistRepository)
                .SetRulesForCreation();

            var validationResult = await _service.AddAsync(entity, validator);
            return validationResult.IsValid
                ? await CommitAsync()
                : await Task.FromResult(validationResult);
        }

        public async Task<IReadOnlyList<MusicCatalogInfoDTO>> GetMusicCatalogsAsync()
        {
            return await _musicCatalogRepository.GetMusicCatalogListAsync();
        }

        public async Task<IReadOnlyList<ArtistInfoDTO>> GetArtistsAsync()
        {
            return await _artistRepository.GetArtistInfosAsync();
        }
    }
}