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
        /// Asynchronously returns a readonly list of <see cref="CatalogDetailDTO"/>.
        /// </summary>
        Task<IReadOnlyList<CatalogDetailDTO>> GetMusicCatalogsAsync();

        /// <summary>
        /// Asynchronously returns a readonly list of <see cref="ArtistDetailsDTO"/>.
        /// </summary>
        Task<IReadOnlyList<ArtistDetailsDTO>> GetArtistsAsync();

        /// <summary>
        /// Asynchronously returns a readonly list of <see cref="AlbumInfoDTO"/>.
        /// </summary>
        Task<IReadOnlyList<AlbumInfoDTO>> GetAlbumsAsync();

        /// <summary>
        /// Asynchronously returns a readonly list of <see cref="SongInfoDTO"/>.
        /// </summary>
        Task<IReadOnlyList<SongInfoDTO>> GetMusicsAsync();

        Task<ValidationResult> RegisterMusicAsync(CreateMusicModel model);

        void SetTargetFullPaths(IEnumerable<CreateMusicModel> models);
    }
}