using FluentValidation.Results;
using Maxsys.MediaManager.MusicContext.ApplicationMVVM.Models;
using Maxsys.MediaManager.MusicContext.Domain.DTO;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Maxsys.MediaManager.MusicContext.ApplicationMVVM.Interfaces.Services
{
    public interface IRegisterMusicAppService : IDisposable
    {
        /// <summary>
        /// Asynchronously returns a readonly list of <see cref="MusicCatalogInfoDTO"/>.
        /// </summary>
        Task<IReadOnlyList<MusicCatalogInfoDTO>> GetMusicCatalogsAsync();

        /// <summary>
        /// Asynchronously returns a readonly list of <see cref="ArtistInfoDTO"/>.
        /// </summary>
        Task<IReadOnlyList<ArtistInfoDTO>> GetArtistsAsync();

        /// <summary>
        /// Asynchronously returns a readonly list of <see cref="AlbumInfoDTO"/>.
        /// </summary>
        Task<IReadOnlyList<AlbumInfoDTO>> GetAlbumsAsync();

        /// <summary>
        /// Asynchronously returns a readonly list of <see cref="MusicInfoDTO"/>.
        /// </summary>
        Task<IReadOnlyList<MusicInfoDTO>> GetMusicsAsync();

        Task<ValidationResult> RegisterMusicAsync(CreateMusicModel model);

        void SetTargetFullPaths(IEnumerable<CreateMusicModel> models);
    }
}