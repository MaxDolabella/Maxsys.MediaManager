using FluentValidation.Results;
using Maxsys.MediaManager.MusicContext.ApplicationMVVM.Models;
using Maxsys.MediaManager.MusicContext.Domain.DTO;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Maxsys.MediaManager.MusicContext.ApplicationMVVM.Interfaces.Services
{
    public interface ICreateMusicCatalogAppService : IDisposable
    {
        Task<ValidationResult> AddNewMusicCatalogAsync(CreateMusicCatalogModel model);

        /// <summary>
        /// Returns a readonly list of <see cref="MusicCatalogInfoModel"/> asynchronous.
        /// </summary>
        Task<IReadOnlyList<MusicCatalogInfoModel>> GetMusicCatalogsAsync();
    }
}