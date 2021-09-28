using Maxsys.Core.Helpers;
using System;
using System.Collections.Generic;

namespace Maxsys.MediaManager.MusicContext.Domain.DTO
{
    public struct Id3v2PlaylistDTO : IEquatable<Id3v2PlaylistDTO>, IEquatable<Id3v2DTO>
    {
        #region CTOR

        /// <summary>
        /// Data transfer object to write tags on playlist musics.
        /// </summary>
        /// <param name="fullPath">is the path where is the file.</param>
        /// <param name="playlistName">is the playlist name.</param>
        /// <param name="musicTitle">is the music title.</param>
        /// <param name="trackOrder">is the music order in playlist.</param>
        /// <param name="stars10">is the music rating number (0-10).</param>
        /// <param name="coverPicture">is the cover picture from music album.</param>
        public Id3v2PlaylistDTO(string fullPath, string playlistName, string musicTitle, uint trackOrder, byte stars10, string genre, byte[] coverPicture)
        {
            FullPath = fullPath;

            Title = musicTitle; // $"{trackOrder:000} {musicTitle}";
            PlaylistName = "_" + playlistName;
            Stars10 = stars10;
            TrackNumber = trackOrder;
            Genre = genre;
            CoverPicture = coverPicture;
        }
        public static readonly Id3v2PlaylistDTO Empty = new Id3v2PlaylistDTO();
        #endregion

        #region PROPS
        public string FullPath { get; }

        public string Title { get; }
        public string PlaylistName { get; }
        public byte Stars10 { get; }
        public uint TrackNumber { get; }
        public string Genre { get; }
        public byte[] CoverPicture { get; }
        #endregion

        #region Equals & operators
        public bool Equals(Id3v2PlaylistDTO other)
        {
            var coverPictureEquals = CoverPicture.ArrayEquals(other.CoverPicture);

            return FullPath == other.FullPath
                && Title == other.Title
                && PlaylistName == other.PlaylistName
                && Stars10 == other.Stars10
                && TrackNumber == other.TrackNumber
                && Genre == other.Genre
                && coverPictureEquals;
        }
        public static bool operator ==(Id3v2PlaylistDTO a, Id3v2PlaylistDTO b) => a.Equals(b);
        public static bool operator !=(Id3v2PlaylistDTO a, Id3v2PlaylistDTO b) => !(a == b);


        public bool Equals(Id3v2DTO other)
        {
            var coverPictureEquals = CoverPicture.ArrayEquals(other.CoverPicture);

            return FullPath == other.FullPath
                && Title == other.Title
                && PlaylistName == other.Artist
                && Stars10 == other.Rating10
                && TrackNumber == other.TrackNumber
                && Genre == other.Genre
                && coverPictureEquals;
        }
        public static bool operator ==(Id3v2PlaylistDTO a, Id3v2DTO b) => a.Equals(b);
        public static bool operator !=(Id3v2PlaylistDTO a, Id3v2DTO b) => !(a == b);
        public static bool operator ==(Id3v2DTO a, Id3v2PlaylistDTO b) => b.Equals(a);
        public static bool operator !=(Id3v2DTO a, Id3v2PlaylistDTO b) => !(b == a);



        public override int GetHashCode() 
            => (FullPath, Title, PlaylistName, Stars10, TrackNumber, Genre, CoverPicture).GetHashCode();
        public override bool Equals(object obj)
        {
            return obj is Id3v2PlaylistDTO tags
                && Equals(tags);
        }
        public override string ToString() => GetType().Name + " [Title=" + Title + "]";

        #endregion
    }
}
