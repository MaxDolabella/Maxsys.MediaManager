using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Maxsys.MediaManager.MusicContext.ApplicationMVVM.Models
{
    public class ArtistListModel : ModelBase
    {
        [Browsable(false)]
        public Guid ArtistId { get; init; }

        [Display(Name = "ARTIST")]
        public string ArtistName { get; init; }

        [Display(Name = "MUSIC CATALOG")]
        public string MusicCatalogName { get; init; }

        [Display(Name = "ALBUMS COUNT")]
        [Range(0, 0, ErrorMessage = "Artist must not contains any album to be deleted.")]
        public int AlbumsCount { get; init; }
    }
}