using Maxsys.MediaManager.MusicContext.ApplicationMVVM.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Maxsys.MediaManager.MusicContext.ApplicationMVVM.Interfaces.Services
{
    public interface IMusicListAppService : IDisposable
    {
        /// <summary>
        /// Asynchronously returns a readonly list of <see cref="MusicListModel"/>.
        /// </summary>
        Task<IReadOnlyList<MusicListModel>> GetMusicsAsync();
    }
}