using System.IO;
using FluentValidation.Results;
using Maxsys.Core.Services;
using Maxsys.MediaManager.MusicContext.Domain.Interfaces.Services;

namespace Maxsys.MediaManager.MusicContext.Domain.Services.PlaylistExporters;

/// <summary>
/// Implements <see cref="IPlaylistFileExporter"/> for M3U playlist format.
/// </summary>
public sealed class M3UPlaylistFileExporter : ServiceBase, IPlaylistFileExporter
{
    public async Task<OperationResult> ExportFileAsync(IEnumerable<string> songFiles, string destFolder, string playlistName, CancellationToken token = default)
    {
        var playlistPath = Path.Combine(destFolder, $"{playlistName}.m3u");
        var contents = new List<string>(songFiles.Count() + 1)
        {
            "#EXTM3U"
        };

        contents.AddRange(songFiles.Select(songPath => Path.GetRelativePath(destFolder.ToString(), songPath.ToString())));

        OperationResult result = new();
        try
        {
            await File.WriteAllLinesAsync(playlistPath.ToString(), contents, token);
        }
        catch (Exception ex)
        {
            // TODO Log???
            result.AddException(ex, "An error ocurred while exporting file");
        }

        return result;
    }
}