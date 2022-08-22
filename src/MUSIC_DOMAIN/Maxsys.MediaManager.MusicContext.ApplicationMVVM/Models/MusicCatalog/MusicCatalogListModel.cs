using Maxsys.MediaManager.MusicContext.Domain.DTO;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Maxsys.MediaManager.MusicContext.ApplicationMVVM.Models
{
    public class MusicCatalogListModel : ValidableModelBase
    {
        private readonly CatalogInfoDTO _musicCatalog;

        public MusicCatalogListModel()
        { }

        public MusicCatalogListModel(CatalogInfoDTO musicCatalog)
        {
            _musicCatalog = musicCatalog;
        }

        [Browsable(false)]
        public Guid MusicCatalogId => _musicCatalog.MusicCatalogId;

        [Display(Name = "MUSIC CATALOG")]
        public string MusicCatalogName => _musicCatalog.MusicCatalogName;

        [Display(Name = "ARTIST COUNT")]
        [Range(0, 0, ErrorMessage = "Song Catalog must not contains any artist to be deleted.")]
        public int ArtistCount => _musicCatalog.ArtistCount;
    }
}