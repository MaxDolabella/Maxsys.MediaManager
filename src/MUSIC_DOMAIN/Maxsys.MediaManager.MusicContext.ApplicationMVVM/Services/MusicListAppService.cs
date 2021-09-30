using Maxsys.AppCore;
using Maxsys.MediaManager.MusicContext.ApplicationMVVM.Interfaces.Services;
using Maxsys.MediaManager.MusicContext.ApplicationMVVM.Models;
using Maxsys.MediaManager.MusicContext.Domain.Interfaces.Repositories;
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
        private readonly IMusicRepository _musicRepository;

        public MusicListAppService(
            ILogger<MusicListAppService> logger,
            IMusicRepository musicRepository)
            : base(null)
        {
            _logger = logger;
            _musicRepository = musicRepository;
        }

        public async Task<IReadOnlyList<MusicListModel>> GetMusicsAsync()
        {
            var dtos = await _musicRepository.GetMusicListAsync();

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
    }
}