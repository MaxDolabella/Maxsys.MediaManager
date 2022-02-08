using FluentValidation.Results;
using Maxsys.MediaManager.MusicContext.Domain.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Maxsys.MediaManager.MusicContext.Domain.Services.PlaylistExporters
{
    /// <summary>
    /// Implements <see cref="IPlaylistFileExporter"/> for M3U playlist format.
    /// </summary>
    public sealed class M3UPlaylistFileExporter : IPlaylistFileExporter
    {
        // WARNING ValueTask ?
        public async Task<ValidationResult> ExportFileAsync(IEnumerable<string> musicFiles, string destFolder, string playlistName)
        {
            var playlistPath = Path.Combine(destFolder, $"{playlistName}.m3u");
            var contents = new List<string>(musicFiles.Count() + 1)
            {
                "#EXTM3U"
            };

            contents.AddRange(musicFiles.Select(musicPath => Path.GetRelativePath(destFolder, musicPath)));

            ValidationResult result;
            try
            {
                await File.WriteAllLinesAsync(playlistPath, contents);

                result = new ValidationResult();
            }
            catch (Exception ex)
            {
                result = await Task.FromResult(ValidationResultExtensions.ValidationResultFromException(ex));
            }

            return result;
        }
    }
}