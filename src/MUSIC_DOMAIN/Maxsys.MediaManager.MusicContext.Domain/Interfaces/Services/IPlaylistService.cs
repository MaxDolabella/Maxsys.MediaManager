using Maxsys.Core.Interfaces.Services;

namespace Maxsys.MediaManager.MusicContext.Domain.Interfaces.Services;

public interface IPlaylistService : IService<Playlist, Guid>
{
    /*
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
    IEnumerable<PlaylistDTO> GetPlaylistsByMusic(Song music);

    */

    Task<OperationResult> ExportPlaylistFileAsync(Guid id, IPlaylistFileExporter playlistFileExporter, Uri? destRootFolder = null, CancellationToken cancellationToken = default);
}