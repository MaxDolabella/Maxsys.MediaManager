using System.IO;
using FluentValidation.Results;
using Maxsys.Core.Services;
using Maxsys.MediaManager.MusicContext.Domain.Interfaces.Services;

namespace Maxsys.MediaManager.MusicContext.Domain.Services.PlaylistExporters;

/// <summary>
/// Implements <see cref="IPlaylistFileExporter"/> for Windows Media Player playlist format (WPL).
/// </summary>
public sealed class WMPPlaylistFileExporter : ServiceBase, IPlaylistFileExporter
{
    public async Task<ValidationResult> ExportFileAsync(IEnumerable<string> songFiles, string destFolder, string playlistName, CancellationToken token = default)
    {
        var path = Path.Combine(destFolder, $"{playlistName}.wpl");
        var contents = GetContents(songFiles
            .Select(songPath => Path.GetRelativePath(destFolder, songPath)), playlistName);

        ValidationResult result = new();
        try
        {
            await File.WriteAllTextAsync(path, contents, token);
        }
        catch (Exception ex)
        {
            // TODO Log???
            result.AddError($"An error ocurred while exporting file: {ex.Message}");
        }

        return result;
    }

    private static string GetContents(IEnumerable<string> songFiles, string playlistName)
    {
        var itemCount = songFiles.Count();
        var srcList = GetSrcList(songFiles);

        return $@"<?wpl version=""1.0""?>
<smil>
    <head>
        <meta name=""Generator"" content=""Maxsys Media Manager""/>
        <meta name=""ItemCount"" content=""{itemCount}""/>
        <title>{playlistName}</title>
    </head>
    <body>
        <seq>
{srcList}
        </seq>
    </body>
</smil>";
    }

    private static string GetSrcList(IEnumerable<string> songFiles)
    {
        var srcs = songFiles
            .Select(x => $@"            <media src=""{x}""/>")
            .Select(x => x.Replace("&", "&amp;").Replace("'", "&apos;"));

        return string.Join(Environment.NewLine, srcs);
    }
}