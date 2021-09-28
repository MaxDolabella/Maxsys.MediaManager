using FluentValidation;
using FluentValidation.Results;
using Maxsys.AppCore;
using Maxsys.DataCore.Interfaces;
using Maxsys.MediaManager.MusicContext.ApplicationMVVM.Interfaces.Services;
using Maxsys.MediaManager.MusicContext.ApplicationMVVM.Models;
using Maxsys.MediaManager.MusicContext.Domain.DTO;
using Maxsys.MediaManager.MusicContext.Domain.Entities;
using Maxsys.MediaManager.MusicContext.Domain.Factories;
using Maxsys.MediaManager.MusicContext.Domain.Interfaces.Mp3;
using Maxsys.MediaManager.MusicContext.Domain.Interfaces.Repositories;
using Maxsys.MediaManager.MusicContext.Domain.Interfaces.Services;
using Maxsys.MediaManager.MusicContext.Domain.Interfaces.Services.Mp3;
using Maxsys.MediaManager.MusicContext.Domain.Validators;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Maxsys.MediaManager.MusicContext.ApplicationMVVM.Services
{
    public sealed class RegisterMusicAppService
        : ApplicationServiceBase, IRegisterMusicAppService
    {
        private readonly ILogger _logger;
        private readonly IMusicService _musicService;
        private readonly IMusicRepository _musicRepository;
        private readonly IMusicCatalogRepository _musicCatalogRepository;
        private readonly IArtistRepository _artistRepository;
        private readonly IAlbumRepository _albumRepository;
        private readonly IPathService _pathService;
        private readonly IMusicPropertiesReader _musicPropertiesReader;
        private readonly ITagService _tagService;

        public RegisterMusicAppService(
            IUnitOfWork uow,
            ILogger<RegisterMusicAppService> logger,
            IMusicService musicService,
            IMusicRepository musicRepository,
            IMusicCatalogRepository musicCatalogRepository,
            IArtistRepository artistRepository,
            IAlbumRepository albumRepository,
            ITagService tagService,
            IPathService pathService,
            IMusicPropertiesReader musicPropertiesReader)
            : base(uow)
        {
            _logger = logger;
            _musicService = musicService;
            _musicRepository = musicRepository;
            _musicCatalogRepository = musicCatalogRepository;
            _artistRepository = artistRepository;
            _albumRepository = albumRepository;
            _tagService = tagService;
            _pathService = pathService;
            _musicPropertiesReader = musicPropertiesReader;
        }

        public async Task<IReadOnlyList<MusicCatalogInfoDTO>> GetMusicCatalogsAsync()
        {
            return await _musicCatalogRepository.GetMusicCatalogListAsync();
        }

        public async Task<IReadOnlyList<ArtistInfoDTO>> GetArtistsAsync()
        {
            return await _artistRepository.GetArtistInfosAsync();
        }

        public async Task<IReadOnlyList<AlbumInfoDTO>> GetAlbumsAsync()
        {
            return await _albumRepository.GetAlbumInfosAsync();
        }

        public async Task<IReadOnlyList<MusicInfoDTO>> GetMusicsAsync()
        {
            return await _musicRepository.GetMusicsAsync();
        }

        public void SetTargetFullPaths(IEnumerable<CreateMusicModel> models)
        {
            foreach (var model in models)
            {
                var dto = new DefineMusicFileNameDTO
                {
                    MusicTrackNumber = model.TrackNumber,
                    MusicTitle = model.Title,
                    MusicIsBonusTrack = model.IsBonusTrack,
                    MusicFeaturedArtist = model.FeaturedArtist,
                    MusicCoveredArtist = model.CoveredArtist,
                    AlbumDirectory = model.Album?.AlbumDirectory
                };

                if (dto.IsValid())
                {
                    var targetFile = _pathService.DefineMusicFilePath(dto);

                    model.SetTargetFullPath(targetFile);
                }
            }
        }

        public async Task<ValidationResult> RegisterMusicAsync(CreateMusicModel model)
        {
            BeginTransaction();

            var validationResult = new ValidationResult();
            var validatorForUpdateMusic = new MusicValidator(_musicRepository).SetRulesForUpdate();
            var validatorForNewMusic = new MusicValidator(_musicRepository).SetRulesForCreation();

            var sourceIsRegistered = await _musicRepository.PathExistsAsync(model.SourceFullPath);
            var targetIsRegistered = await _musicRepository.PathExistsAsync(model.TargetFullPath);

            if (targetIsRegistered)
            {
                if (!sourceIsRegistered)
                    return await ReplaceExistingFileAsync(model, validatorForUpdateMusic);

                validationResult.AddFailure(nameof(model.TargetFullPath),
                    $"The target path \"{model.TargetFullPath}\" cannot be registered if the source path \"{model.SourceFullPath}\" is already registered.");
            }
            else
            {
                validationResult = await CheckTrackAndTitleAsync(model);
                if (validationResult.IsValid)
                {
                    return sourceIsRegistered
                        ? await ChangeAlbumAsync(model, validatorForUpdateMusic)
                        : await AddNewMusicAsync(model, validatorForNewMusic);
                }
            }

            return validationResult;
        }

        /// <summary>
        /// Adds a new music into repository, tags id3v2 into file and moves file into music library.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        private async Task<ValidationResult> AddNewMusicAsync(CreateMusicModel model, IValidator<Music> validator)
        {
            _logger.LogDebug("Adding music async");

            var originalFileName = _musicPropertiesReader.GetFileNameWithoutExtension(model.SourceFullPath);
            var fileSize = _musicPropertiesReader.GetFileSize(model.SourceFullPath);
            var duration = _musicPropertiesReader.GetMusicDuration(model.SourceFullPath);
            var bitrate = _musicPropertiesReader.GetMusicBitrate(model.SourceFullPath);
            IEnumerable<Composer> composers = null;

            var newMusic = MusicFactory.Create(
                model.Id,
                model.Album.AlbumId,
                originalFileName,
                model.TargetFullPath,
                model.Title,
                model.TrackNumber,
                model.Lyrics,
                model.Comments,
                model.IsBonusTrack,
                model.VocalGender,
                model.CoveredArtist,
                model.FeaturedArtist,
                model.Rating,
                fileSize,
                duration,
                bitrate,
                composers);

            // ADDING
            _logger.LogDebug("Adding new music.");
            var validationResult = await _musicService.AddAsync(newMusic, validator);
            if (!validationResult.IsValid) return validationResult;

            _logger.LogDebug("Music added.");

            // TAGGING
            _logger.LogDebug("Tagging music.");
            var (musicTagDTO, albumTagDTO) = await GetMusicAndAlbumTagDTOFromModel(model);
            validationResult = _tagService.WriteTags(Id3v2DTO.FromDTOs(musicTagDTO, albumTagDTO));
            if (!validationResult.IsValid) return validationResult;

            _logger.LogDebug("Music tagged.");

            // MOVING
            _logger.LogDebug("Moving to music to library.");
            validationResult = await _musicService.MoveToLibraryAsync(model.SourceFullPath, model.TargetFullPath);
            if (!validationResult.IsValid) return validationResult;

            _logger.LogDebug("Music moved to library.");

            // COMMITTING
            validationResult = await CommitAsync();

            return validationResult;
        }

        /// <summary>
        /// Replaces a music file to another keeping tags.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        private async Task<ValidationResult> ReplaceExistingFileAsync(CreateMusicModel model, IValidator<Music> validator)
        {
            // WARNING Se eu marcar por exemplo como bonus track, dá erro de Title Exists

            _logger.LogDebug("Replacing existing music async");

            var existingMusic = await _musicService.GetByPathAsync(model.TargetFullPath);
            existingMusic.UpdateFilePropertiesFrom(_musicPropertiesReader, model.SourceFullPath);

            // UPDATING
            _logger.LogDebug("Updating music.");
            var validationResult = await _musicService.UpdateAsync(existingMusic, validator);
            if (!validationResult.IsValid) return validationResult;

            _logger.LogDebug("Music updated.");

            // TAGGING
            _logger.LogDebug("Tagging music.");
            var id3Tags = _tagService.ReadTags(model.TargetFullPath).ToFile(model.SourceFullPath);
            validationResult = _tagService.WriteTags(id3Tags);
            if (!validationResult.IsValid) return validationResult;

            _logger.LogDebug("Music tagged.");

            // MOVING
            _logger.LogDebug("Replacing music file in library.");
            validationResult = await _musicService.ReplaceToLibraryAsync(model.SourceFullPath, model.TargetFullPath);
            if (!validationResult.IsValid) return validationResult;

            _logger.LogDebug("Music file replaced to library.");

            // COMMITTING
            validationResult = await CommitAsync();

            return validationResult;
        }

        /// <summary>
        /// Gets an existing music in library and move to another album. Also updates the tags.
        /// </summary>
        /// <param name="model"></param>
        private async Task<ValidationResult> ChangeAlbumAsync(CreateMusicModel model, IValidator<Music> validator)
        {
            _logger.LogDebug("Changing album async.");

            var music = await _musicService.GetByPathAsync(model.SourceFullPath);
            var album = await _albumRepository.GetAlbumTagDTO(model.AlbumId);

            music.UpdateFullPath(model.TargetFullPath);
            music.UpdateTrackNumber(model.TrackNumber);
            music.UpdateAlbum(model.AlbumId);

            var updatedTags = _tagService
                .ReadTags(model.SourceFullPath)
                .UpdateAlbum(album)
                .UpdateTrackNumber(model.TrackNumber);

            // UPDATING
            _logger.LogDebug("Updating music.");
            var validationResult = await _musicService.UpdateAsync(music, validator);
            if (!validationResult.IsValid) return validationResult;

            _logger.LogDebug("Music updated.");

            // TAGGING
            _logger.LogDebug("Tagging music");
            validationResult = _tagService.WriteTags(updatedTags);
            if (!validationResult.IsValid) return validationResult;

            _logger.LogDebug("Music tagged.");

            // MOVING
            _logger.LogDebug("Moving to music to library.");
            validationResult = await _musicService.MoveToLibraryAsync(model.SourceFullPath, model.TargetFullPath);
            if (!validationResult.IsValid) return validationResult;

            _logger.LogDebug("Music moved to library.");

            // COMMITTING
            validationResult = await CommitAsync();

            return validationResult;
        }

        /// <summary>
        /// Checks wether TrackNumber or Title already exists in Album.
        /// </summary>
        /// <param name="model"></param>
        /// <returns>ValidationResult with errors when TrackNumber or Title are already registered</returns>
        private async Task<ValidationResult> CheckTrackAndTitleAsync(CreateMusicModel model)
        {
            var albumsTrackTitle = await _albumRepository.GetTracksAndTitlesAsync(model.AlbumId);
            var sameTitle = albumsTrackTitle.Any(x => x.Title == model.Title);
            var sameTrack = albumsTrackTitle.Any(t => t.Track.HasValue && t.Track == model.TrackNumber);

            var validationResult = new ValidationResult();
            if (sameTitle) validationResult
                    .AddFailure(nameof(model.Title), $"A music with same Title <{model.Title}> already exists in Album.");
            if (sameTrack) validationResult
                    .AddFailure(nameof(model.TrackNumber), $"A music with same Track Number <{model.TrackNumber}> already exists in Album");

            return validationResult;
        }

        private async Task<(MusicTagDTO, AlbumTagDTO)> GetMusicAndAlbumTagDTOFromModel(CreateMusicModel model)
        {
            var musicTagDTO = new MusicTagDTO
            {
                MusicFullPath = model.SourceFullPath,
                MusicTitle = model.Title,
                MusicTrackNumber = model.TrackNumber,
                MusicRating10 = model.Rating,
                MusicComments = model.Comments,
                MusicLyrics = model.Lyrics,
                // TODO Add Composers in CreateMusicModel
                MusicComposers = Array.Empty<string>(),// model.Composers
                MusicCoveredArtist = model.CoveredArtist,
                MusicFeaturedArtist = model.FeaturedArtist
            };
            var albumTagDTO = await _albumRepository.GetAlbumTagDTO(model.AlbumId);

            return (musicTagDTO, albumTagDTO);
        }
    }
}