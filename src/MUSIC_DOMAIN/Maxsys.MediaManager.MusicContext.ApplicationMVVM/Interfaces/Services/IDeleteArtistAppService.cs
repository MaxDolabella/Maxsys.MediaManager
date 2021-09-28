using FluentValidation.Results;
using Maxsys.MediaManager.MusicContext.ApplicationMVVM.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Maxsys.MediaManager.MusicContext.ApplicationMVVM.Interfaces.Services
{
    public interface IDeleteArtistAppService : IDisposable
    {
        Task<ValidationResult> DeleteArtistAsync(ArtistListModel model);

        /// <summary>
        /// Returns a readonly list of <see cref="ArtistListModel"/> asynchronous.
        /// </summary>
        Task<IReadOnlyList<ArtistListModel>> GetArtistsAsync();
    }
}