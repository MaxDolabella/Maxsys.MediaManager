using CommunityToolkit.Mvvm.ComponentModel;
using Maxsys.MediaManager.MusicContext.Domain.DTO;
using System;

namespace Maxsys.MediaManager.MusicContext.ApplicationMVVM.Models
{
    /// <summary>
    /// Observable wrapper to <see cref="MusicCatalogInfoDTO"/>.
    /// </summary>
    public class MusicCatalogInfoModel : ObservableObject
    {
        private readonly MusicCatalogInfoDTO _musicCatalog;

        public MusicCatalogInfoModel(MusicCatalogInfoDTO musicCatalog)
        {
            _musicCatalog = musicCatalog;
        }

        public Guid MusicCatalogId => _musicCatalog.MusicCatalogId;
        public string MusicCatalogName => _musicCatalog.MusicCatalogName;
    }
}