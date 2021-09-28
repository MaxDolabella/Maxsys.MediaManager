using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Maxsys.MediaManager.MusicContext.ApplicationMVVM.Models
{
    public class MusicCatalogListModel : ModelBase
    {
        [Browsable(false)]
        public Guid MusicCatalogId { get; init; }

        [Display(Name = "MUSIC CATALOG")]
        public string MusicCatalogName { get; init; }

        [Display(Name = "ARTIST COUNT")]
        [Range(0, 0, ErrorMessage = "Music Catalog must not contains any artist to be deleted.")]
        public int ArtistCount { get; init; }
    }
}