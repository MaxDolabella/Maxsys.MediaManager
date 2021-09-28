using Maxsys.Core.Helpers;
using Maxsys.MediaManager.MusicContext.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Maxsys.MediaManager.MusicContext.Domain.ValueObjects
{
    public struct Id3v2TagsDTO : IEquatable<Id3v2TagsDTO>
    {
        #region CTOR
        public Id3v2TagsDTO(string fullPath, string title, byte stars10, string album
            , string[] genres, string[] performers, uint? trackNumber, uint? year
            , string comments, string lyrics, string coveredArtist, string featuredArtist
            , string[] composers, byte[] coverPicture)
        {
            FullPath = fullPath ?? throw new ArgumentNullException(nameof(fullPath));
            Title = title ?? throw new ArgumentNullException(nameof(title));
            Performers = performers ?? throw new ArgumentNullException(nameof(performers));
            Album = album ?? throw new ArgumentNullException(nameof(album));
            Genres = genres ?? throw new ArgumentNullException(nameof(genres));
            Stars10 = stars10;
            TrackNumber = trackNumber;
            Year = year;
            Comments = comments;
            Lyrics = lyrics;
            CoveredArtist = coveredArtist;
            FeaturedArtist = featuredArtist;
            Composers = composers ?? throw new ArgumentNullException(nameof(composers));
            CoverPicture = coverPicture ?? throw new ArgumentNullException(nameof(coverPicture));
        }
        #endregion

        #region FACTORY
        public static Id3v2TagsDTO FromMusic(Music music)
        {
            var album = music.Album;
            var artist = album.Artist;

            var fullPath = music.FullPath;
            var title = music.Title;
            var performers = new string[] { artist.Name };
            var albumName = album.Name;
            var genres = new string[] { album.Genre };
            var stars10 = music.Classification.GetStars10();
            var trackNumber = (uint?)music.TrackNumber;
            var year = (uint?)album.Year;
            var comments = music.Comments;
            var lyrics = music.Lyrics;

            var coveredArtist = music.MusicDetails.CoveredArtist;
            var featuredArtist = music.MusicDetails.FeaturedArtist;

            var coverPicture = album.AlbumCover;
            var composers = music.Composers.Select(c => c.Name).ToArray();

            return new Id3v2TagsDTO(fullPath, title, stars10
                , albumName, genres, performers
                , /*Nullable values*/trackNumber, year, comments, lyrics, coveredArtist, featuredArtist
                , composers, coverPicture);
        }
        
        public static readonly Id3v2TagsDTO Empty = new Id3v2TagsDTO();
        #endregion

        #region PROPS
        public string FullPath { get; private set; }

        // Not nullable values

        /// <summary>Not nullable</summary>
        public string Title { get; }
        
        /// <summary>Not nullable</summary>
        public string[] Performers { get; private set; }
        
        /// <summary>Not nullable</summary>
        public string[] AlbumArtists => Performers;
        
        /// <summary>Not nullable</summary>
        public string Album { get; private set; }

        /// <summary>Not nullable</summary>
        public string[] Genres { get; private set; }
        public byte Stars10 { get; private set; }

        /// <summary>Not nullable</summary>
        public string[] Composers { get; private set; }

        /// <summary>Not nullable</summary>
        public byte[] CoverPicture { get; private set; }

        // Nullable values
        public uint? TrackNumber { get; private set; }
        public uint? Year { get; private set; }

        /// <summary>Nullable</summary>
        public string Comments { get; private set; }
        /// <summary>Nullable</summary>
        public string CoveredArtist { get; private set; }
        /// <summary>Nullable</summary>
        public string FeaturedArtist { get; private set; }

        /// <summary>Nullable</summary>
        public string Lyrics { get; private set; }

        #endregion

        #region Equals & operators
        public bool Equals(Id3v2TagsDTO other)
        {
            var performersEquals = Performers.ArrayEquals(other.Performers);
            var genresEquals = Genres.ArrayEquals(other.Genres);
            var composersEquals = Composers.ArrayEquals(other.Composers);
            var coverPictureEquals = CoverPicture.ArrayEquals(other.CoverPicture);

            return FullPath == other.FullPath
                && Title == other.Title
                && Album == other.Album
                && Stars10 == other.Stars10
                && TrackNumber == other.TrackNumber
                && Year == other.Year
                && Lyrics == other.Lyrics
                && CoveredArtist == other.CoveredArtist
                && FeaturedArtist == other.FeaturedArtist
                && performersEquals
                && genresEquals
                && composersEquals
                && coverPictureEquals;
        }

        public override bool Equals(object obj) => obj is Id3v2TagsDTO tags && Equals(tags);
        public static bool operator ==(Id3v2TagsDTO a, Id3v2TagsDTO b) => a.Equals(b);
        public static bool operator !=(Id3v2TagsDTO a, Id3v2TagsDTO b) => !(a == b);
        public override int GetHashCode()
        {
            var hashcode = (FullPath, Title, Performers, Album, Genres, Stars10
                , TrackNumber, Year, Composers, Lyrics, CoverPicture).GetHashCode();
            
            return hashcode;
        }
        public override string ToString()
        {
            return GetType().Name + " [Title=" + Title + "]";
        }
        #endregion

        #region Methods
        public Id3v2TagsDTO UpdateAlbum(Album album)
        {
            if (this == Empty) return Empty;

            Album = album?.Name ?? throw new ArgumentNullException(nameof(album));
            
            Performers = new string[] { album.Artist.Name };
            Genres = new string[] { album.Genre };
            Year = (uint?)album.Year;
            CoverPicture = album.AlbumCover;

            return this;
        }

        public Id3v2TagsDTO UpdateTrackNumber(int? trackNumber)
        {
            if (this == Empty) return Empty;

            TrackNumber = (uint?)trackNumber;

            return this;
        }

        public Id3v2TagsDTO ToFile(string fileToTag)
        {
            if (this == Empty) return Empty;

            FullPath = fileToTag;

            return this;
        }
        #endregion
    }
}
