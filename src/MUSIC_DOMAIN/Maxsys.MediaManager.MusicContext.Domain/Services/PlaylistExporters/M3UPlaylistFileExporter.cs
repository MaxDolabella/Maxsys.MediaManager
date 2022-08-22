using FluentValidation.Results;
using Maxsys.MediaManager.MusicContext.Domain.Interfaces.Services;
using System.IO;

namespace Maxsys.MediaManager.MusicContext.Domain.Services.PlaylistExporters;

/// <summary>
/// Implements <see cref="IPlaylistFileExporter"/> for M3U playlist format.
/// </summary>
public sealed class M3UPlaylistFileExporter : IPlaylistFileExporter
{
    public async Task<ValidationResult> ExportFileAsync(IEnumerable<string> songFiles, string destFolder, string playlistName, CancellationToken token = default)
    {
        var playlistPath = Path.Combine(destFolder, $"{playlistName}.m3u");
        var contents = new List<string>(songFiles.Count() + 1)
        {
            "#EXTM3U"
        };

        contents.AddRange(songFiles.Select(songPath => Path.GetRelativePath(destFolder, songPath)));

        ValidationResult result = new();
        try
        {
            await File.WriteAllLinesAsync(playlistPath, contents, token);
        }
        catch (Exception ex)
        {
            // TODO Log???
            result.AddErrorMessage($"An error ocurred while exporting file: {ex.Message}");
        }

        return result;
    }
}