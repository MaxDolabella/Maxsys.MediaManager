using FluentValidation.Results;
using Maxsys.MediaManager.MusicContext.Domain.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Maxsys.MediaManager.MusicContext.Domain.Services.PlaylistExporters
{
    public sealed class M3UPlaylistFileExporter : IPlaylistFileExporter
    {
        // WARNING ValueTask ?
        public async Task<ValidationResult> ExportFile(IEnumerable<string> musicFiles, string destFolder, string playlistName)
        {
            var path = Path.Combine(destFolder, $"{playlistName}.m3u");
            var contents = new List<string>(musicFiles.Count() + 1)
            {
                "#EXTM3U"
            };
            contents.AddRange(musicFiles);

            // WARNING for now, use absolute path. Later, use relative path.

            ValidationResult result;
            try
            {
                await File.WriteAllLinesAsync(path, contents);

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