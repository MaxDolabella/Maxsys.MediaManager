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
        /// Returns a readonly list of <see cref="ArtistInfoModel"/> asynchronous.
        /// </summary>
        Task<IReadOnlyList<ArtistInfoModel>> GetArtistsAsync();

        /// <summary>
        /// Returns a readonly list of <see cref="AlbumInfoModel"/> asynchronous.
        /// </summary>
        Task<IReadOnlyList<AlbumInfoModel>> GetAlbumsAsync();

        Task<IReadOnlyList<string>> GetGenresAsync();
    }
}