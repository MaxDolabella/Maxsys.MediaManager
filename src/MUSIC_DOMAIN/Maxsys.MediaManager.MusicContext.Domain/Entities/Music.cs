using Maxsys.MediaManager.CoreDomain;
using Maxsys.MediaManager.MusicContext.Domain.Interfaces.Services.Mp3;
using Maxsys.MediaManager.MusicContext.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Maxsys.MediaManager.MusicContext.Domain.Entities
{
    [DebuggerDisplay("{Id.ToString().Substring(0, 4)} - {Title}")]
    public class Music : MediaFile
    {
        #region PROPERTIES

        public string Title { get; protected set; }
        public int? TrackNumber { get; protected set; }
        public string Lyrics { get; protected set; }
        public string Comments { get; protected set; }

        // Value Objects
        public Classification Classification { get; protected set; }

        public MusicDetails MusicDetails { get; protected set; }
        public MusicProperties MusicProperties { get; protected set; }

        // Navigation
        public Guid AlbumId { get; protected set; }

        public virtual Album Album { get; protected set; }

        // Collections
        //public virtual ICollection<MusicComposer> MusicComposer { get; } = new List<MusicComposer>();
        public virtual ICollection<Composer> Composers { get; protected set; }// = new List<Composer>();

        //public virtual ICollection<Playlist> Playlists { get; protected set; }

        #endregion PROPERTIES

        #region CONSTRUCTORS

        protected Music()
        {
            Composers = new List<Composer>();
            //MusicComposer = new List<MusicComposer>();
            //Playlists = new List<Playlist>();
        }

        internal Music(
            Guid id,
            Guid albumId,
            string originalFileName,
            string fullPath,
            long fileSize,
            string title,
            int? trackNumber,
            string lyrics,
            string comments,
            MusicDetails musicDetails,
            Classification classification,
            MusicProperties musicProperties)
            : base(id, fullPath, originalFileName, fileSize)
        {
            AlbumId = albumId;

            Title = title;
            TrackNumber = trackNumber;
            Lyrics = lyrics;
            Comments = comments;

            MusicDetails = musicDetails;
            Classification = classification;
            MusicProperties = musicProperties;
        }

        #endregion CONSTRUCTORS

        #region METHODS

        public void AddComposer(Composer composer)
        {
            if (Composers == null) Composers = new List<Composer>();

            if (!Composers.Any(x => x.Id == composer.Id))
                Composers.Add(composer);

            /* Old version to EF Core < 5
            if (MusicComposer.Any(x => x.ComposerId == composer.Id))
                return;

            MusicComposer.Add(new MusicComposer
            {
                Composer = composer,
                Music = this
            });
            */
        }

        public void UpdateFilePropertiesFrom(IMusicPropertiesReader musicPropertiesReader, string replacingMusicPath)
        {
            // MediaFile
            this.OriginalFileName = musicPropertiesReader.GetFileNameWithoutExtension(replacingMusicPath);
            this.FileSize = musicPropertiesReader.GetFileSize(replacingMusicPath);

            // MusicProperties
            var duration = musicPropertiesReader.GetMusicDuration(replacingMusicPath);
            var bitrate = musicPropertiesReader.GetMusicBitrate(replacingMusicPath);

            var newMusicProps = new MusicProperties(duration, bitrate);

            this.MusicProperties.UpdateFrom(newMusicProps);
        }

        public void UpdateRatingPoints(int newValue) => Classification.UpdateRating(newValue);

        public void UpdateFullPath(string newFullPath) => FullPath = newFullPath;

        public void UpdateAlbum(Album newAlbum)
        {
            Album = newAlbum;
            AlbumId = newAlbum.Id;
        }

        public void UpdateAlbum(Guid newAlbumId)
        {
            AlbumId = newAlbumId;
        }

        public void UpdateTrackNumber(int? trackNumber) => TrackNumber = trackNumber;

        //public string ComposersToString()
        //{
        //    return (MusicComposer is null)
        //        ? string.Empty
        //        : string.Join("; ", System.Linq.Enumerable.Select(MusicComposer, x => x.Composer.Name));
        //}

        ///// <summary>
        ///// Update <see cref="MediaFile.OriginalFileName"/>, <see cref="MediaFile.FileSize"/> and <see cref="Music.MusicProperties"/>
        ///// </summary>
        ///// <param name="musicPropertiesReader"></param>
        ///// <param name="source"></param>
        //public void UpdateFilePropertiesFrom(IMusicPropertiesReader musicPropertiesReader, Music source)
        //{
        //    this.OriginalFileName = musicPropertiesReader.GetFileNameWithoutExtension(source.FullPath);
        //    this.FileSize = musicPropertiesReader.GetFileSize(source.FullPath);

        //    this.MusicProperties.UpdateFrom(source.MusicProperties);
        //}

        //public void UpdateFilePropertiesFrom(Music source)
        //{
        //    this.UpdateMediaFileFrom(source);
        //    this.UpdateMusicPropertiesFrom(source.MusicProperties);
        //}
        //public void UpdateMusicPropertiesFrom(MusicProperties source)
        //{
        //    this.MusicProperties.UpdateFrom(source);
        //}
        //public void UpdateFilePropertiesFrom(string filePath, MusicProperties source)
        //{
        //    this.UpdateMediaFileFrom(filePath);
        //    this.UpdateMusicPropertiesFrom(source);
        //}

        #endregion METHODS
    }
}