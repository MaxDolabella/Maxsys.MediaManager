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

            Composers = new List<Composer>();
            //Playlists = new List<Playlist>();
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

        
        #endregion METHODS
    }
}