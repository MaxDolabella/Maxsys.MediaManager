using Maxsys.Core.Helpers;
using Maxsys.MediaManager.MusicContext.Domain.DTO;
using Maxsys.MediaManager.MusicContext.Domain.Interfaces.Services;
using Maxsys.MediaManager.MusicContext.Domain.Options;
using Maxsys.MediaManager.MusicContext.Domain.ValueObjects;
using Microsoft.Extensions.Options;
using System;
using System.IO;

namespace Maxsys.MediaManager.MusicContext.Domain.Services
{
    ///<inheritdoc cref="IPathService"/>
    public sealed class PathService : IPathService
    {
        private readonly MusicSettings _musicSettingsOptions;

        public PathService(IOptions<MusicSettings> musicSettingsOptions)
        {
            _musicSettingsOptions = musicSettingsOptions.Value;
        }

        public string DefineAlbumDirectory(DefineAlbumDirectoryDTO dto)
        {
            if (!dto.IsValid()) throw new ArgumentException($"{nameof(DefineAlbumDirectoryDTO)} must be valid.");

            var libFolder = _musicSettingsOptions.MusicLibraryFolder;
            var artistDirectory = Path.Combine(libFolder,
                                               dto.MusicCatalogName,
                                               dto.ArtistName);

            var albumTypeFolder = dto.AlbumType switch
            {
                AlbumType.Studio => "Studio",
                AlbumType.Live => "Live",
                AlbumType.Compilation => "Compilation",
                AlbumType.Bootleg => "Bootleg",
                AlbumType.Others => "Others",
                //AlbumType.Undefined or AlbumType.Various => string.Empty
                _ => string.Empty
            };

            var name = IOHelper.ReplaceAndRemoveInvalidDirectoryChars(dto.AlbumName);
            var albumFolder = dto.AlbumYear.HasValue
                ? $"({dto.AlbumYear.Value}) {name}"
                : name;

            return Path.Combine(artistDirectory, albumTypeFolder, albumFolder);
        }

        public string DefineMusicFilePath(DefineMusicFileNameDTO dto)
        {
            if (!dto.IsValid()) throw new ArgumentException($"{nameof(DefineAlbumDirectoryDTO)} must be valid.");

            var fileName = DefineMusicFileName(dto);

            return Path.Combine(dto.AlbumDirectory, fileName + ".mp3");
        }

        public string DefineMusicFileName(DefineMusicFileNameDTO dto)
        {
            if (!dto.IsValid()) throw new ArgumentException($"{nameof(DefineAlbumDirectoryDTO)} must be valid.");

            var track = dto.MusicTrackNumber.HasValue
                ? $"{dto.MusicTrackNumber.Value:00} " : string.Empty;

            var bonus = dto.MusicIsBonusTrack
                ? $" [Bonus]" : string.Empty;

            var featured = !string.IsNullOrWhiteSpace(dto.MusicFeaturedArtist)
                ? $" (feat. {dto.MusicFeaturedArtist})"
                : string.Empty;
            var covered = !string.IsNullOrWhiteSpace(dto.MusicCoveredArtist)
                ? $" ({dto.MusicCoveredArtist} Cover)"
                : string.Empty;

            var fileName = $@"{track}{dto.MusicTitle}{bonus}{featured}{covered}"
                .Replace(")  (", " - ").Replace("  ", "");

            return IOHelper.ReplaceAndRemoveInvalidFileNameChars(fileName);
        }

        public string GetMusicCatalogDirectory(string musicCatalogName)
        {
            if (string.IsNullOrWhiteSpace(musicCatalogName))
                throw new ArgumentException($"'{nameof(musicCatalogName)}' must be valid.",
                    nameof(musicCatalogName));

            return Path.Combine(_musicSettingsOptions.MusicLibraryFolder, musicCatalogName);
        }

        public string GetArtistDirectory(DefineArtistFolderDTO dto)
        {
            if (!dto.IsValid())
                throw new ArgumentException($"'{nameof(dto)}' must be valid.", nameof(dto));

            return Path.Combine(GetMusicCatalogDirectory(dto.MusicCatalogName), dto.ArtistName);
        }
    }
}