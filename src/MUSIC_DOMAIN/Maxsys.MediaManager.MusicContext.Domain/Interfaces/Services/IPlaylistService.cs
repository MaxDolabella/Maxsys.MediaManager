using FluentValidation.Results;
using Maxsys.Core.Interfaces.Services;

namespace Maxsys.MediaManager.MusicContext.Domain.Interfaces.Services;

public interface IPlaylistService : IService
{
    ///// <summary>
    ///// Write Id3 tags on musics from a playlist and copies the files to a specific folder
    ///// </summary>
    ///// <param name="playlist">is a <see cref="Playlist"/></param>
    ///// <param name="destRootFolder">is the directory that will contains the mp3 files from playlist</param>
    //ValidationResult ExportPlaylist(in Playlist playlist, string destRootFolder);

    ///// <summary>
    ///// Get collection of Playlist that contains a given music
    ///// </summary>
    ///// <param name="music"></param>
    ///// <returns></returns>
    //IEnumerable<PlaylistDTO> GetPlaylistsByMusic(Song music);
    Task<ValidationResult> ExportPlaylistFileAsync(Playlist playlist, IPlaylistFileExporter playlistFileExporter, string? destRootFolder = null, CancellationToken token = default);
}