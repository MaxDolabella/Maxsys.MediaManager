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
        /// Returns a readonly list of <see cref="MusicCatalogInfoModel"/> asynchronous.
        /// </summary>
        Task<IReadOnlyList<MusicCatalogInfoModel>> GetMusicCatalogsAsync();

        /// <summary>
        /// Returns a readonly list of <see cref="ArtistInfoModel"/> asynchronous.
        /// </summary>
        Task<IReadOnlyList<ArtistInfoModel>> GetArtistsAsync();
    }
}