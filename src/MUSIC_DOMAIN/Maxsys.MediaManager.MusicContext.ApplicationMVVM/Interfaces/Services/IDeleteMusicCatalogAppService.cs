using FluentValidation.Results;
using Maxsys.MediaManager.MusicContext.ApplicationMVVM.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Maxsys.MediaManager.MusicContext.ApplicationMVVM.Interfaces.Services
{
    public interface IDeleteMusicCatalogAppService : IDisposable
    {
        Task<ValidationResult> DeleteMusicCatalogAsync(MusicCatalogListModel model);

        /// <summary>
        /// Returns a readonly list of <see cref="MusicCatalogListModel"/> asynchronous.
        /// </summary>
        Task<IReadOnlyList<MusicCatalogListModel>> GetMusicCatalogsAsync();
    }
}