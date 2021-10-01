using FluentValidation.Results;
using Maxsys.AppCore;
using Maxsys.Core.Helpers;
using Maxsys.DataCore.Interfaces;
using Maxsys.MediaManager.MusicContext.ApplicationMVVM.Interfaces.Services;
using Maxsys.MediaManager.MusicContext.ApplicationMVVM.Models;
using Maxsys.MediaManager.MusicContext.Domain.Interfaces.Repositories;
using Maxsys.MediaManager.MusicContext.Domain.Interfaces.Services;
using Maxsys.MediaManager.MusicContext.Domain.ValueObjects;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Maxsys.MediaManager.MusicContext.ApplicationMVVM.Services
{
    public sealed class MusicListAppService
        : ApplicationServiceBase, IMusicListAppService
    {
        private readonly ILogger _logger;
        private readonly IMusicService _service;
        private readonly IMusicRepository _repository;
        private readonly IPlaylistRepository _playlistRepository;

        public MusicListAppService(IUnitOfWork uow,
            ILogger<MusicListAppService> logger,
            IMusicService service,
            IMusicRepository repository,
            IPlaylistRepository playlistRepository)
            : base(uow)
        {
            _logger = logger;
            _service = service;
            _repository = repository;
            _playlistRepository = playlistRepository;
        }

        public async Task<IReadOnlyList<MusicListModel>> GetMusicsAsync()
        {
            var dtos = await _repository.GetMusicListAsync();

            return dtos.Select(dto => new MusicListModel
            {
                MusicCatalogName = dto.MusicCatalogName,
                ArtistName = dto.ArtistName,
                AlbumName = dto.AlbumName,
                AlbumType = dto.AlbumType.ToFriendlyName(),

                MusicId = dto.MusicId,
                MusicFullPath = dto.MusicFullPath,
                MusicTitle = dto.MusicTitle,
                MusicTrackNumber = dto.MusicTrackNumber,
                MusicVocalGender = dto.MusicVocalGender.ToString(),
                MusicRating = dto.MusicRating,
                MusicFeaturedArtist = dto.MusicFeaturedArtist ?? string.Empty,
                MusicCoveredArtist = dto.MusicCoveredArtist ?? string.Empty,
                IsMusicCover = !string.IsNullOrWhiteSpace(dto.MusicCoveredArtist)
            })
            .OrderBy(m => m.MusicFullPath)
            .ToList();
        }

        public async Task<ValidationResult> DeleteMusicAsync(MusicListModel model)
        {
            BeginTransaction();

            var entityForDeletion = await _service.GetByIdAsync(model.MusicId, @readonly: false);

            // Deletion
            var validationResult = await _service.RemoveAsync(entityForDeletion.Id);
            if (!validationResult.IsValid) return validationResult;

            // Playlists
            validationResult = await RemovePlaylistItemsAsync(model);
            if (!validationResult.IsValid) return validationResult;

            return validationResult.IsValid
                ? await CommitAsync()
                : validationResult;
        }

        private async Task<ValidationResult> RemovePlaylistItemsAsync(MusicListModel model)
        {
            var validationResult = new ValidationResult();

            var playlistItems = await _playlistRepository.GetPlaylistItemsByMusicIdAsync(model.MusicId, @readonly: false);

            if (playlistItems.Any())
            {
                var isAllItemsSettedToRemove = _playlistRepository.RemovePlaylistItems(playlistItems);
                if (!isAllItemsSettedToRemove)
                {
                    validationResult.AddFailure("One or more playlist item cannot be removed.");
                }
            }

            return validationResult;
        }

        public async Task DeleteMusicFileAsync(MusicListModel model)
        {
            await Task.Run(() =>
            {
                try
                {
                    _logger.LogDebug($"Deleting music <{model.MusicFullPath}>.");

                    _ = IOHelper.DeleteFileAsync(model.MusicFullPath)
                    .ConfigureAwait(false);

                    _logger.LogWarning($"Music deleted.");
                }
                catch (System.Exception ex)
                {
                    _logger.LogError("Music cannot be deleted:\n{errors}", ex.Message);
                }
            });
        }
    }
}