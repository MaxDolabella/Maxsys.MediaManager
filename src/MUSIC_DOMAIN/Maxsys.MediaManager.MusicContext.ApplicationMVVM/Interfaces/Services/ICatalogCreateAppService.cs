using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FluentValidation.Results;
using Maxsys.MediaManager.MusicContext.ApplicationMVVM.Interfaces.ViewModels;
using Maxsys.MediaManager.MusicContext.ApplicationMVVM.ViewModels.Catalog;

namespace Maxsys.MediaManager.MusicContext.ApplicationMVVM.Interfaces.Services
{
    public interface ICatalogCreateAppService : IDisposable
    {
        Task<ValidationResult> CreateCatalogAsync(ICatalogCreateViewModel viewModel);

        /// <summary>
        /// Returns a readonly list of <see cref="CatalogDetailViewModel"/> asynchronous.
        /// </summary>
        Task<IReadOnlyList<CatalogDetailViewModel>> GetMusicCatalogsAsync();
    }
}