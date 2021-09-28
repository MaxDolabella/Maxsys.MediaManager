﻿using System;
using System.Collections.Generic;

namespace Maxsys.MediaManager.MusicContext.Domain.DTO
{
    public struct Id3v2DTO : IEquatable<Id3v2DTO>
    {
        #region CTOR

        public Id3v2DTO(
            string fullPath,
            string title,
            byte stars10,
            string album,
            string genre,
            string artist,
            int? trackNumber,
            int? year,
            string comments,
            string lyrics,
            string coveredArtist,
            string featuredArtist,
            string[] composers,
            byte[] coverPicture)
        {
            FullPath = fullPath ?? throw new ArgumentNullException(nameof(fullPath));
            Title = title ?? throw new ArgumentNullException(nameof(title));
            Artist = artist ?? throw new ArgumentNullException(nameof(artist));
            Album = album ?? throw new ArgumentNullException(nameof(album));
            Genre = genre ?? throw new ArgumentNullException(nameof(genre));
            Rating10 = stars10 <= 10 ? stars10 : throw new ArgumentOutOfRangeException(nameof(stars10), $"Value <{stars10}> must be lower or equal to 10");
            TrackNumber = trackNumber;
            Year = year;
            Comments = comments;
            Lyrics = lyrics;
            CoveredArtist = coveredArtist;
            FeaturedArtist = featuredArtist;
            Composers = composers ?? throw new ArgumentNullException(nameof(composers));
            CoverPicture = coverPicture ?? throw new ArgumentNullException(nameof(coverPicture));
        }

        #endregion CTOR

        #region FACTORY

        public static Id3v2DTO FromDTOs(MusicTagDTO music, AlbumTagDTO album)
        {
            return new Id3v2DTO(
                music.MusicFullPath,
                music.MusicTitle,
                music.MusicRating10,
                album.AlbumName,
                album.AlbumGenre,
                album.ArtistName,
                music.MusicTrackNumber,
                album.AlbumYear,
                music.MusicComments,
                music.MusicLyrics,
                music.MusicCoveredArtist,
                music.MusicFeaturedArtist,
                music.MusicComposers,
                album.AlbumCover);
        }

        public static readonly Id3v2DTO Empty = new Id3v2DTO();

        #endregion FACTORY

        #region PROPS

        public string FullPath { get; private set; }

        // Not nullable values

        /// <summary>Not nullable</summary>
        public string Title { get; }

        /// <summary>Not nullable</summary>
        public string Artist { get; private set; }

        /// <summary>Not nullable</summary>
        public string Album { get; private set; }

        /// <summary>Not nullable</summary>
        public string Genre { get; private set; }

        public byte Rating10 { get; private set; }

        /// <summary>Not nullable</summary>
        public string[] Composers { get; private set; }

        /// <summary>Not nullable</summary>
        public byte[] CoverPicture { get; private set; }

        // Nullable values
        public int? TrackNumber { get; private set; }

        public int? Year { get; private set; }

        /// <summary>Nullable</summary>
        public string Comments { get; private set; }

        /// <summary>Nullable</summary>
        public string CoveredArtist { get; private set; }

        /// <summary>Nullable</summary>
        public string FeaturedArtist { get; private set; }

        /// <summary>Nullable</summary>
        public string Lyrics { get; private set; }

        #endregion PROPS

        #region Equals & operators

        public bool Equals(Id3v2DTO other)
        {
            var composersEquals = Composers.ArrayEquals(other.Composers);
            var coverPictureEquals = CoverPicture.ArrayEquals(other.CoverPicture);

            return FullPath == other.FullPath
                && Title == other.Title
                && Album == other.Album
                && Genre == other.Genre
                && Artist == other.Artist
                && Rating10 == other.Rating10
                && TrackNumber == other.TrackNumber
                && Year == other.Year
                && Lyrics == other.Lyrics
                && CoveredArtist == other.CoveredArtist
                && FeaturedArtist == other.FeaturedArtist
                && composersEquals
                && coverPictureEquals;
        }

        public override bool Equals(object obj) => obj is Id3v2DTO tags && Equals(tags);

        public static bool operator ==(Id3v2DTO a, Id3v2DTO b) => a.Equals(b);

        public static bool operator !=(Id3v2DTO a, Id3v2DTO b) => !(a == b);

        public override int GetHashCode()
        {
            var hashcode = (FullPath, Title, Artist, Album, Genre, Rating10
                , TrackNumber, Year, Composers, Lyrics, CoverPicture).GetHashCode();

            return hashcode;
        }

        public override string ToString()
        {
            return GetType().Name + " [Title=" + Title + "]";
        }

        #endregion Equals & operators

        #region Methods

        public Id3v2DTO UpdateAlbum(AlbumTagDTO dto)
        {
            if (this == Empty) return Empty;
            if (!dto.IsValid()) throw new ArgumentException("UpdateAlbumTagDTO is invalid.");

            Album = dto.AlbumName;
            Artist = dto.ArtistName;
            Genre = dto.AlbumGenre;
            Year = dto.AlbumYear;
            CoverPicture = dto.AlbumCover;

            return this;
        }

        public Id3v2DTO UpdateTrackNumber(int? trackNumber)
        {
            if (this == Empty) return Empty;

            TrackNumber = trackNumber;

            return this;
        }

        public Id3v2DTO ToFile(string fileToTag)
        {
            if (this == Empty) return Empty;

            FullPath = fileToTag;

            return this;
        }

        #endregion Methods
    }
}