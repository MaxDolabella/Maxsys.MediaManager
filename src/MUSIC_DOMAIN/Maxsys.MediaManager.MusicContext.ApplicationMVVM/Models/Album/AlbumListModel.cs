using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Maxsys.MediaManager.MusicContext.ApplicationMVVM.Models
{
    public class AlbumListModel : ModelBase
    {
        [Display(Name = "MUSIC CATALOG")]
        public string AlbumMusicCatalogName { get; init; }

        [Display(Name = "ARTIST")]
        public string AlbumArtistName { get; init; }

        [Browsable(false)]
        public Guid AlbumId { get; init; }

        [Display(Name = "ALBUM")]
        public string AlbumName { get; init; }

        [Display(Name = "TYPE")]
        public string AlbumType { get; init; }

        [Display(Name = "YEAR")]
        public string AlbumYear { get; init; }

        [Display(Name = "MUSIC COUNT")]
        [Range(0, 0, ErrorMessage = "Album must not contains any music to be deleted.")]
        public int AlbumMusicCount { get; init; }

        [Browsable(false)]
        public string AlbumDirectory { get; init; }
    }
}