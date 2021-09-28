using FluentValidation.Results;
using Maxsys.MediaManager.MusicContext.ApplicationMVVM.Models;
using Maxsys.MediaManager.MusicContext.Domain.DTO;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Maxsys.MediaManager.MusicContext.ApplicationMVVM.Interfaces.Services
{
    public interface ICreateAlbumAppService : IDisposable
    {
        Task<ValidationResult> AddNewAlbumAsync(CreateAlbumModel model);

        /// <summary>
        /// Returns a readonly list of <see cref="ArtistInfoDTO"/> asynchronous.
        /// </summary>
        Task<IReadOnlyList<ArtistInfoDTO>> GetArtistsAsync();

        /// <summary>
        /// Returns a readonly list of <see cref="AlbumInfoDTO"/> asynchronous.
        /// </summary>
        Task<IReadOnlyList<AlbumInfoDTO>> GetAlbumsAsync();

        Task<IReadOnlyList<string>> GetGenresAsync();
    }
}