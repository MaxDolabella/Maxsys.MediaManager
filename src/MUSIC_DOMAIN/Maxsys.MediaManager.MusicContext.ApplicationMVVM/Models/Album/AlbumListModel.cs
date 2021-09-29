using Maxsys.MediaManager.MusicContext.Domain.DTO;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Maxsys.MediaManager.MusicContext.ApplicationMVVM.Models
{
    public class AlbumListModel : ModelBase
    {
        private readonly AlbumListDTO _album;
        public AlbumListModel()
        { }
        
        public AlbumListModel(AlbumListDTO album)
        {
            _album = album;
        }

        [Display(Name = "MUSIC CATALOG")]
        public string AlbumMusicCatalogName => _album.AlbumMusicCatalogName;

        [Display(Name = "ARTIST")]
        public string AlbumArtistName => _album.AlbumArtistName;

        [Browsable(false)]
        public Guid AlbumId => _album.AlbumId;

        [Display(Name = "ALBUM")]
        public string AlbumName => _album.AlbumName;

        [Display(Name = "TYPE")]
        public string AlbumType => _album.AlbumType.ToString();

        [Display(Name = "YEAR")]
        public string AlbumYear => _album.AlbumYear.HasValue
            ? _album.AlbumYear.Value.ToString() : string.Empty;

        [Display(Name = "MUSIC COUNT")]
        [Range(0, 0, ErrorMessage = "Album must not contains any music to be deleted.")]
        public int AlbumMusicCount => _album.AlbumMusicCount;

        [Browsable(false)]
        public string AlbumDirectory => _album.AlbumDirectory;
    }
}