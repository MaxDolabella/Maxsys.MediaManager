using CommunityToolkit.Mvvm.ComponentModel;
using Maxsys.MediaManager.MusicContext.Domain.DTO;
using System;

namespace Maxsys.MediaManager.MusicContext.ApplicationMVVM.Models
{
    /// <summary>
    /// Observable wrapper to <see cref="CatalogDetailDTO"/>.
    /// </summary>
    public class MusicCatalogInfoModel : ObservableObject
    {
        private readonly CatalogDetailDTO _musicCatalog;

        public MusicCatalogInfoModel(CatalogDetailDTO musicCatalog)
        {
            _musicCatalog = musicCatalog;
        }

        public Guid MusicCatalogId => _musicCatalog.MusicCatalogId;
        public string MusicCatalogName => _musicCatalog.MusicCatalogName;
    }
}