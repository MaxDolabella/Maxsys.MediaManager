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
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Maxsys.MediaManager.MusicContext.ApplicationMVVM.Services
{
    public sealed class CreateAlbumAppService
        : ApplicationServiceBase, ICreateAlbumAppService
    {
        private readonly ILogger _logger;
        private readonly IAlbumService _service;
        private readonly IAlbumRepository _albumRepository;
        private readonly IArtistRepository _artistRepository;
        private readonly IPathService _pathService;

        public CreateAlbumAppService(IUnitOfWork uow,
            IAlbumService service,
            IArtistRepository artistRepository,
            IAlbumRepository albumRepository,
            IPathService pathService,
            ILogger<CreateAlbumAppService> logger)
            : base(uow)
        {
            _service = service;
            _albumRepository = albumRepository;
            _artistRepository = artistRepository;
            _pathService = pathService;
            _logger = logger;
        }

        public async Task<ValidationResult> AddNewAlbumAsync(CreateAlbumModel model)
        {
            BeginTransaction();

            var defineAlbumDirectoryDTO = new DefineAlbumDirectoryDTO
            {
                MusicCatalogName = model.Artist.MusicCatalogName,
                ArtistName = model.Artist.ArtistName,
                AlbumName = model.Name,
                AlbumYear = model.Year,
                AlbumType = model.AlbumType
            };

            var albumDirectory = _pathService.DefineAlbumDirectory(defineAlbumDirectoryDTO);

            var entity = AlbumFactory.Create(
                model.ArtistId,
                albumDirectory,
                model.Name,
                model.Year,
                model.Genre,
                model.AlbumCover,
                model.AlbumType);

            var validator = new AlbumValidator(_albumRepository).SetRulesForCreation();

            var validationResult = await _service.AddAsync(entity, validator);

            if (validationResult.IsValid)
            {
                validationResult = await CommitAsync();

                if (validationResult.IsValid)
                    _ = _service.SaveCoverPictureAsync(albumDirectory, model.AlbumCover).ConfigureAwait(false);
            }

            return validationResult;
        }

        public async Task<IReadOnlyList<ArtistInfoModel>> GetArtistsAsync()
        {
            var dtos = await _artistRepository.GetArtistDetailsAsync();

            return dtos.Select(dto => new ArtistInfoModel(dto)).ToList();
        }

        public async Task<IReadOnlyList<AlbumInfoModel>> GetAlbumsAsync()
        {
            var dtos = await _albumRepository.GetAlbumInfosAsync();

            return dtos.Select(dto => new AlbumInfoModel(dto)).ToList();
        }

        public async Task<IReadOnlyList<string>> GetGenresAsync()
        {
            return await _albumRepository.GetGenresAsync();
        }
    }
}