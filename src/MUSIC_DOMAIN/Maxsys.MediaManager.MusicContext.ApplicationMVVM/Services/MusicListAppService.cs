//using FluentValidation.Results;
//using Maxsys.AppCore;
//using Maxsys.Core.Helpers;
//using Maxsys.DataCore.Interfaces;
//using Maxsys.MediaManager.MusicContext.ApplicationMVVM.Interfaces.Services;
//using Maxsys.MediaManager.MusicContext.ApplicationMVVM.Models;
//using Maxsys.MediaManager.MusicContext.Domain.Interfaces.Repositories;
//using Maxsys.MediaManager.MusicContext.Domain.Interfaces.Services;
//using Maxsys.MediaManager.MusicContext.Domain.ValueObjects;
//using Microsoft.Extensions.Logging;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;

//namespace Maxsys.MediaManager.MusicContext.ApplicationMVVM.Services
//{
//    public sealed class MusicListAppService
//        : ApplicationServiceBase, IMusicListAppService
//    {
//        private readonly ILogger _logger;
//        private readonly ISongService _service;
//        private readonly ISongRepository _repository;
//        private readonly IPlaylistRepository _playlistRepository;

//        public MusicListAppService(IUnitOfWork uow,
//            ILogger<MusicListAppService> logger,
//            ISongService service,
//            ISongRepository repository,
//            IPlaylistRepository playlistRepository)
//            : base(uow)
//        {
//            _logger = logger;
//            _service = service;
//            _repository = repository;
//            _playlistRepository = playlistRepository;
//        }

//        public async Task<IReadOnlyList<MusicListModel>> GetMusicsAsync()
//        {
//            var dtos = await _repository.GetSongDetailsAsync();

//            return dtos.Select(dto => new MusicListModel
//            {
//                MusicCatalogName = dto.CatalogName,
//                ArtistName = dto.ArtistName,
//                AlbumName = dto.AlbumName,
//                AlbumType = dto.AlbumType.ToFriendlyName(),

//                MusicId = dto.MusicId,
//                MusicFullPath = dto.SongFullPath,
//                MusicTitle = dto.MusicTitle,
//                MusicTrackNumber = dto.MusicTrackNumber,
//                MusicVocalGender = dto.SongVocalGender.ToString(),
//                MusicRating = dto.SongRating,
//                MusicFeaturedArtist = dto.MusicFeaturedArtist ?? string.Empty,
//                MusicCoveredArtist = dto.MusicCoveredArtist ?? string.Empty,
//                IsMusicCover = !string.IsNullOrWhiteSpace(dto.MusicCoveredArtist)
//            })
//            .OrderBy(m => m.MusicFullPath)
//            .ToList();
//        }

//        public async Task<ValidationResult> DeleteMusicAsync(MusicListModel model)
//        {
//            BeginTransaction();

//            var entityForDeletion = await _service.GetByIdAsync(model.MusicId, @readonly: false);

//            // Deletion
//            var validationResult = await _service.RemoveAsync(entityForDeletion.Id);
//            if (!validationResult.IsValid) return validationResult;

//            // Playlists
//            validationResult = await RemovePlaylistItemsAsync(model);
//            if (!validationResult.IsValid) return validationResult;

//            return validationResult.IsValid
//                ? await CommitAsync()
//                : validationResult;
//        }

//        private async Task<ValidationResult> RemovePlaylistItemsAsync(MusicListModel model)
//        {
//            var validationResult = new ValidationResult();

//            var playlistItems = await _playlistRepository.GetPlaylistItemsBySongIdAsync(model.MusicId, @readonly: false);

//            if (playlistItems.Any())
//            {
//                var isAllItemsSettedToRemove = _playlistRepository.RemovePlaylistItems(playlistItems);
//                if (!isAllItemsSettedToRemove)
//                {
//                    validationResult.AddFailure("One or more playlist item cannot be removed.");
//                }
//            }

//            return validationResult;
//        }

//        public async Task DeleteMusicFileAsync(MusicListModel model)
//        {
//            await Task.Run(() =>
//            {
//                try
//                {
//                    _logger.LogDebug($"Deleting music <{model.MusicFullPath}>.");

//                    _ = IOHelper.DeleteFileAsync(model.MusicFullPath)
//                    .ConfigureAwait(false);

//                    _logger.LogWarning($"Song deleted.");
//                }
//                catch (System.Exception ex)
//                {
//                    _logger.LogError("Song cannot be deleted:\n{errors}", ex.Message);
//                }
//            });
//        }
//    }
//}