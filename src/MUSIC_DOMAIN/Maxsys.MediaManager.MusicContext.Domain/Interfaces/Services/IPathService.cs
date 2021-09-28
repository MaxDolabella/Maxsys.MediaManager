using Maxsys.MediaManager.MusicContext.Domain.DTO;
using Maxsys.MediaManager.MusicContext.Domain.Entities;

namespace Maxsys.MediaManager.MusicContext.Domain.Interfaces.Services
{
    /// <summary>
    /// Provides methods for handle paths and file names for musics, albums and artists.
    /// </summary>
    public interface IPathService
    {
        /// <summary>
        /// Define the full album directory based on it's info, Artist and Music Catalog.<br/>
        /// Examples:
        /// <code>
        /// DefineAlbumDirectoryDTO dto;
        /// dto.MusicCatalogName = "CATALOG";
        /// dto.ArtistName = "Artist01";
        /// dto.AlbumName = "Album 123";
        /// dto.AlbumType = "Studio";
        /// dto.AlbumYear = "1999";
        ///
        /// D:\MusicLibraryFolder\CATALOG\Artist01\Studio\(1999) Album 123\
        /// </code>
        /// </summary>
        /// <param name="dto">DTO with information for path creation.</param>
        /// <returns>The folder path of the artist.</returns>
        string DefineAlbumDirectory(DefineAlbumDirectoryDTO dto);

        /// <summary>
        /// Define a mp3 file name wihtout extension given parameters.<br/>
        /// Examples:
        /// <code>"03 Music"
        /// "Other Song (SomeArtist Cover)"</code>
        /// </summary>
        /// <param name="dto"></param>
        /// <returns>a music file name.</returns>
        string DefineMusicFileName(DefineMusicFileNameDTO dto);

        /// <summary>
        /// Define a mp3 (full) file name given parameters.<br/>
        /// Examples:
        /// <code>"D:\Musics\CATALOG\Artist01\Studio\(1999) Album 123\03 Music.mp3"
        /// "D:\Musics\CATALOG\Artist01\Album xpto\Other Song (SomeArtist Cover).mp3"</code>
        /// </summary>
        /// <param name="dto"></param>
        /// <returns>a music file path.</returns>
        string DefineMusicFilePath(DefineMusicFileNameDTO dto);

        /// <summary>
        /// Define and gets the directory of a specific <see cref="MusicCatalog"/>.<br/>
        /// Examples:
        /// <code>"D:\Musics\ROCKCATALOG"</code>
        /// </summary>
        /// <param name="musicCatalogName">is the name of the <see cref="MusicCatalog"/>
        /// to retrieve the directory.</param>
        /// <returns>the directory of the <see cref="MusicCatalog"/>.</returns>
        string GetMusicCatalogDirectory(string musicCatalogName);

        /// <summary>
        /// Define and gets the directory of a specific <see cref="Artist"/>.<br/>
        /// Examples:
        /// <code>"D:\Musics\ROCKCATALOG\Rock Band"</code>
        /// </summary>
        /// <param name="dto">Is the data transfer object containing the
        /// <see cref="Artist.Name"/> and <see cref="MusicCatalog.Name"/>
        /// to retrieve the directory.</param>
        /// <returns>the directory of the <see cref="Artist"/>.</returns>
        string GetArtistDirectory(DefineArtistFolderDTO dto);
    }
}