using Maxsys.MediaManager.MusicContext.Domain.DTO;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Maxsys.MediaManager.MusicContext.ApplicationMVVM.Models
{
    public class ArtistListModel : ValidableModelBase
    {
        private readonly ArtistListDTO _artist;

        public ArtistListModel()
        { }

        public ArtistListModel(ArtistListDTO artist)
            => _artist = artist;

        [Browsable(false)]
        public Guid ArtistId => _artist.ArtistId;

        [Display(Name = "ARTIST")]
        public string ArtistName => _artist.ArtistName;

        [Display(Name = "MUSIC CATALOG")]
        public string MusicCatalogName => _artist.MusicCatalogName;

        [Display(Name = "ALBUMS COUNT")]
        [Range(0, 0, ErrorMessage = "Artist must not contains any album to be deleted.")]
        public int AlbumsCount => _artist.AlbumsCount;
    }
}