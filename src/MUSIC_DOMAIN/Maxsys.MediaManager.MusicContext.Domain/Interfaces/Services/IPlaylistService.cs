using FluentValidation.Results;
using Maxsys.MediaManager.MusicContext.Domain.DTO;
using Maxsys.MediaManager.MusicContext.Domain.Entities;
using Maxsys.ModelCore.Interfaces.Services;
using System;
using System.Collections.Generic;

namespace Maxsys.MediaManager.MusicContext.Domain.Interfaces.Services
{
    public interface IPlaylistService : IServiceBase<Playlist>
    {
        /// <summary>
        /// Write Id3 tags on musics from a playlist and copies the files to a specific folder
        /// </summary>
        /// <param name="playlist">is a <see cref="Playlist"/></param>
        /// <param name="destRootFolder">is the directory that will contains the mp3 files from playlist</param>
        ValidationResult ExportPlaylist(in Playlist playlist, string destRootFolder);

        /// <summary>
        /// Get collection of Playlist that contains a given music
        /// </summary>
        /// <param name="music"></param>
        /// <returns></returns>
        IEnumerable<PlaylistDTO> GetPlaylistsByMusic(Music music);

        
    }
}