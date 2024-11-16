//using CommunityToolkit.Mvvm.ComponentModel;
//using Maxsys.MediaManager.MusicContext.Domain.DTO;
//using System;

//namespace Maxsys.MediaManager.MusicContext.ApplicationMVVM.Models
//{
//    /// <summary>
//    /// Observable wrapper to <see cref="AlbumInfoDTO"/>.
//    /// </summary>
//    public class AlbumInfoModel : ObservableObject
//    {
//        private readonly AlbumInfoDTO _album;

//        public AlbumInfoModel(AlbumInfoDTO album)
//        {
//            _album = album;
//        }

//        public Guid AlbumId => _album.AlbumId;
//        public Guid ArtistId => _album.ArtistId;
//        public string AlbumName => _album.AlbumName;
//        public string ArtistName => _album.ArtistName;
//        public string AlbumDirectory => _album.AlbumDirectory;
//    }
//}