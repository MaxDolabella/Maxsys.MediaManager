using FluentValidation.Results;
using Maxsys.MediaManager.MusicContext.ApplicationMVVM.Models;
using Maxsys.MediaManager.MusicContext.Domain.DTO;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Maxsys.MediaManager.MusicContext.ApplicationMVVM.Interfaces.Services
{
    public interface ICreateArtistAppService : IDisposable
    {
        Task<ValidationResult> AddNewArtistAsync(CreateArtistModel model);

        /// <summary>
        /// Returns a readonly list of <see cref="MusicCatalogInfoDTO"/> asynchronous.
        /// </summary>
        Task<IReadOnlyList<MusicCatalogInfoDTO>> GetMusicCatalogsAsync();

        /// <summary>
        /// Returns a readonly list of <see cref="ArtistInfoDTO"/> asynchronous.
        /// </summary>
        Task<IReadOnlyList<ArtistInfoDTO>> GetArtistsAsync();
    }
}