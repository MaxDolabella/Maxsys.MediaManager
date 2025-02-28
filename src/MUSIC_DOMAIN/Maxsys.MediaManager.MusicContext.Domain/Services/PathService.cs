using System.IO;
using Maxsys.Core.Helpers;
using Maxsys.Core.Services;
using Maxsys.MediaManager.MusicContext.Domain.DTO;
using Maxsys.MediaManager.MusicContext.Domain.Enums;
using Maxsys.MediaManager.MusicContext.Domain.Interfaces.Services;
using Maxsys.MediaManager.MusicContext.Domain.Options;
using Microsoft.Extensions.Options;

namespace Maxsys.MediaManager.MusicContext.Domain.Services;

///<inheritdoc cref="IPathService"/>
public sealed class PathService : ServiceBase, IPathService
{
    private readonly MusicSettings _musicSettings;

    public PathService(IOptions<MusicSettings> musicSettingsOptions)
    {
        _musicSettings = musicSettingsOptions.Value;
    }

    public Uri DefineAlbumDirectory(DefineAlbumDirectoryParams dto)
    {
        if (!dto.IsValid()) throw new ArgumentException($"{nameof(DefineAlbumDirectoryParams)} must be valid.");

        var libFolder = _musicSettings.MusicLibraryFolder;
        var artistDirectory = Path.Combine(libFolder.AbsolutePath,
                                           dto.MusicCatalogName,
                                           dto.ArtistName);

        var albumTypeFolder = dto.AlbumType switch
        {
            AlbumTypes.Studio => "Studio",
            AlbumTypes.Live => "Live",
            AlbumTypes.Compilation => "Compilation",
            AlbumTypes.Bootleg => "Bootleg",
            AlbumTypes.Others => "Others",
            //AlbumType.Undefined or AlbumType.Various => string.Empty
            _ => string.Empty
        };

        var name = IOHelper.ReplaceAndRemoveInvalidDirectoryChars(dto.AlbumName);
        var albumFolder = dto.AlbumYear.HasValue
            ? $"({dto.AlbumYear.Value}) {name}"
            : name;

        return new Uri(Path.Combine(artistDirectory, albumTypeFolder, albumFolder));
    }

    public Uri DefineSongFilePath(DefineSongFileNameParams dto)
    {
        if (!dto.IsValid()) throw new ArgumentException($"{nameof(DefineAlbumDirectoryParams)} must be valid.");

        var fileName = DefineSongFileName(dto);

        return new Uri(Path.Combine(dto.AlbumDirectory, fileName + ".mp3"));
    }

    public string DefineSongFileName(DefineSongFileNameParams dto)
    {
        if (!dto.IsValid()) throw new ArgumentException($"{nameof(DefineAlbumDirectoryParams)} must be valid.");

        var track = dto.SongTrackNumber.HasValue
            ? $"{dto.SongTrackNumber.Value:00} " : string.Empty;

        var bonus = dto.SongIsBonusTrack
            ? $" [Bonus]" : string.Empty;

        var featured = !string.IsNullOrWhiteSpace(dto.SongFeaturedArtist)
            ? $" (feat. {dto.SongFeaturedArtist})"
            : string.Empty;
        var covered = !string.IsNullOrWhiteSpace(dto.SongCoveredArtist)
            ? $" ({dto.SongCoveredArtist} Cover)"
            : string.Empty;

        var fileName = $@"{track}{dto.SongTitle}{bonus}{featured}{covered}"
            .Replace(")  (", " - ").Replace("  ", "");

        return IOHelper.ReplaceAndRemoveInvalidFileNameChars(fileName);
    }

    public Uri GetCatalogDirectory(string musicCatalogName)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(musicCatalogName, nameof(musicCatalogName));

        return new Uri(Path.Combine(_musicSettings.MusicLibraryFolder.AbsolutePath, musicCatalogName));
    }

    public Uri GetArtistDirectory(DefineArtistFolderParams dto)
    {
        return new Uri(Path.Combine(GetCatalogDirectory(dto.MusicCatalogName).AbsolutePath, dto.ArtistName));
    }

    public Uri GetDefaultPlaylistDirectory()
    {
        return new Uri(Path.Combine(_musicSettings.MusicLibraryFolder.AbsolutePath, _musicSettings.PlaylistFolderName));
    }
}