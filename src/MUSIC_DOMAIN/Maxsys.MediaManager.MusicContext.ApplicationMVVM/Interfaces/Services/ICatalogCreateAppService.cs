using FluentValidation.Results;
using Maxsys.MediaManager.MusicContext.ApplicationMVVM.Models;
using Maxsys.MediaManager.MusicContext.ApplicationMVVM.ViewModels.Catalog;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Maxsys.MediaManager.MusicContext.ApplicationMVVM.Interfaces.Services
{
    public interface ICatalogCreateAppService : IDisposable
    {
        Task<ValidationResult> CreateCatalogAsync(CatalogCreateViewModel viewModel);

        /// <summary>
        /// Returns a readonly list of <see cref="CatalogDetailViewModel"/> asynchronous.
        /// </summary>
        Task<IReadOnlyList<CatalogDetailViewModel>> GetMusicCatalogsAsync();
    }
}