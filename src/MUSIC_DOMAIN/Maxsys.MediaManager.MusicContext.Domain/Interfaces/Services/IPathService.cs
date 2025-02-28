using Maxsys.Core.Interfaces.Services;
using Maxsys.MediaManager.MusicContext.Domain.DTO;

namespace Maxsys.MediaManager.MusicContext.Domain.Interfaces.Services;

// TODO replaces strings to URI?
/// <summary>
/// Provides methods for handle paths and file names for musics, albums and artists.
/// </summary>
public interface IPathService : IService
{
    /// <summary>
    /// Define the full album directory based on it's info, Artist and Song Catalog.<br/>
    /// Examples:
    /// <code>
    /// DefineAlbumDirectoryDTO dto;
    /// dto.CatalogName = "CATALOG";
    /// dto.Artist = "Artist01";
    /// dto.Album = "Album 123";
    /// dto.AlbumType = "Studio";
    /// dto.AlbumYear = "1999";
    ///
    /// D:\MusicLibraryFolder\CATALOG\Artist01\Studio\(1999) Album 123\
    /// </code>
    /// </summary>
    /// <param name="dto">DTO with information for path creation.</param>
    /// <returns>The folder path of the artist.</returns>
    Uri DefineAlbumDirectory(DefineAlbumDirectoryParams dto);

    /// <summary>
    /// Define a mp3 file name wihtout extension given parameters.<br/>
    /// Examples:
    /// <code>"03 Song"
    /// "Other Song (SomeArtist Cover)"</code>
    /// </summary>
    /// <param name="dto"></param>
    /// <returns>a music file name.</returns>
    string DefineSongFileName(DefineSongFileNameParams dto);

    /// <summary>
    /// Define a mp3 (full) file name given parameters.<br/>
    /// Examples:
    /// <code>"D:\Songs\CATALOG\Artist01\Studio\(1999) Album 123\03 Song.mp3"
    /// "D:\Songs\CATALOG\Artist01\Album xpto\Other Song (SomeArtist Cover).mp3"</code>
    /// </summary>
    /// <param name="dto"></param>
    /// <returns>a music file path.</returns>
    Uri DefineSongFilePath(DefineSongFileNameParams dto);

    /// <summary>
    /// Define and gets the directory of a specific <see cref="Catalog"/>.<br/>
    /// Examples:
    /// <code>"D:\Songs\ROCKCATALOG"</code>
    /// </summary>
    /// <param name="catalogName">is the name of the <see cref="Catalog"/>
    /// to retrieve the directory.</param>
    /// <returns>the directory of the <see cref="Catalog"/>.</returns>
    Uri GetCatalogDirectory(string catalogName);

    /// <summary>
    /// Define and gets the directory of a specific <see cref="Artist"/>.<br/>
    /// Examples:
    /// <code>"D:\Songs\ROCKCATALOG\Rock Band"</code>
    /// </summary>
    /// <param name="dto">Is the data transfer object containing the
    /// <see cref="Artist.Name"/> and <see cref="Catalog.Name"/>
    /// to retrieve the directory.</param>
    /// <returns>the directory of the <see cref="Artist"/>.</returns>
    Uri GetArtistDirectory(DefineArtistFolderParams dto);

    /// <summary>
    /// Gets the default playist directory.
    /// <br/><br/>
    /// The pattern is "[MusicLibraryFolder]\Playlists"
    /// <br/>
    /// Example:
    /// <code>"D:\Songs\Playlists\"</code>
    /// </summary>
    /// <returns>the default playist directory.</returns>
    Uri GetDefaultPlaylistDirectory();
}