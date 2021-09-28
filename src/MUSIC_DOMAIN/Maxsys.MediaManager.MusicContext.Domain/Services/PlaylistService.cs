using FluentValidation.Results;
using Maxsys.Core.Helpers;
using Maxsys.MediaManager.MusicContext.Domain.DTO;
using Maxsys.MediaManager.MusicContext.Domain.Entities;
using Maxsys.MediaManager.MusicContext.Domain.Interfaces.Mp3;
using Maxsys.MediaManager.MusicContext.Domain.Interfaces.Repositories;
using Maxsys.MediaManager.MusicContext.Domain.Interfaces.Services;
using Maxsys.ModelCore.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Maxsys.MediaManager.MusicContext.Domain.Services
{
    public class PlaylistService : ServiceBase<Playlist>, IPlaylistService
    {
        private readonly IPlaylistRepository _repository;
        private readonly ITagService _tagService;

        #region CONTRUCTORS

        public PlaylistService(
            IPlaylistRepository repository,
            ITagService tagService)
            : base(repository)
        {
            _repository = repository;
            _tagService = tagService;
        }

        #endregion CONTRUCTORS

        /// <inheritdoc/>
        public ValidationResult ExportPlaylist(in Playlist playlist, string destRootFolder)
        {
            var validationResult = new ValidationResult();

            // Creates a Playlist Folder to put the mp3 files
            var playlistName = playlist.Name.ToUpper();
            var playlistFolder = Path.Combine(destRootFolder, $"_PL {playlistName}");

            Directory.CreateDirectory(playlistFolder);

            playlist.Items.Sort();

            for (int index = 0; index < playlist.Items.Count; index++)
            {
                #region Setting

                var music = playlist.Items[index].Music;

                var trackOrder = Convert.ToUInt32(index + 1);
                var musicTitle = $"{trackOrder:000} {music.Title}";
                var stars10 = music.Classification.GetStars10();
                var genre = music.Album.Genre;
                var coverPicture = music.Album.AlbumCover;

                var fullPath = Path.Combine(playlistFolder, $"{trackOrder:000} {music.Id}.mp3");

                var tags = new Id3v2PlaylistDTO(fullPath, playlistName, musicTitle, trackOrder, stars10, genre, coverPicture);

                #endregion Setting

                #region Copying

                // Copy mp3 files to the Playlist folder. Returns validation if some item fails.
                var srcFile = music.FullPath;
                var cpyFile = tags.FullPath;

                var copyingResult = IOHelper.CopyFile(srcFile, cpyFile);
                if (!copyingResult.IsValid) return copyingResult;

                #endregion Copying

                #region Tagging

                // Sets the tags to the copied file. Returns validation if some item fails
                var taggingResult = _tagService.WritePlaylistTags(tags);
                if (!taggingResult.IsValid) return taggingResult;

                #endregion Tagging
            }

            return validationResult;
        }

        /// <inheritdoc/>
        public IEnumerable<PlaylistDTO> GetPlaylistsByMusic(Music music)
        {
            var playlists = _repository
                .Find(pl => pl.Items.Contains(music), @readonly: true)
                .Select(pl => new PlaylistDTO
                {
                    PlaylistId = pl.Id,
                    PlaylistName = pl.Name
                });

            return playlists;
        }

        public Playlist GetByIdWithDependencies(Guid id)
        {
            var pl = _repository.GetByIdWithDependencies(id);

            if (pl is not null) pl.Items.Sort();

            return pl;
        }
    }
}