using CommunityToolkit.Mvvm.ComponentModel;
using Maxsys.MediaManager.MusicContext.Domain.DTO;
using System;

namespace Maxsys.MediaManager.MusicContext.ApplicationMVVM.Models
{
    /// <summary>
    /// Observable wrapper to <see cref="ArtistInfoDTO"/>.
    /// </summary>
    public class ArtistInfoModel : ObservableObject
    {
        private readonly ArtistInfoDTO _artist;

        public ArtistInfoModel(ArtistInfoDTO artist)
        {
            _artist = artist;
        }

        public Guid MusicCatalogId => _artist.MusicCatalogId;
        public Guid ArtistId => _artist.ArtistId;
        public string ArtistName => _artist.ArtistName;
        public string MusicCatalogName => _artist.MusicCatalogName;
    }
}