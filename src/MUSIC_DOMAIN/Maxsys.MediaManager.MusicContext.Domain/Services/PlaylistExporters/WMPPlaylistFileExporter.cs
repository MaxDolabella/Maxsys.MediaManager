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
    /// Implements <see cref="IPlaylistFileExporter"/> for Windows Media Player playlist format (WPL).
    /// </summary>
    public sealed class WMPPlaylistFileExporter : IPlaylistFileExporter
    {
        // WARNING ValueTask ?
        public async Task<ValidationResult> ExportFileAsync(IEnumerable<string> musicFiles, string destFolder, string playlistName)
        {
            var path = Path.Combine(destFolder, $"{playlistName}.wpl");
            var contents = GetContents(musicFiles
                .Select(musicPath => Path.GetRelativePath(destFolder, musicPath)), playlistName);

            ValidationResult result;
            try
            {
                await File.WriteAllTextAsync(path, contents);

                result = new ValidationResult();
            }
            catch (Exception ex)
            {
                result = await Task.FromResult(ValidationResultExtensions.ValidationResultFromException(ex));
            }

            return result;
        }

        private static string GetContents(IEnumerable<string> musicFiles, string playlistName)
        {
            var itemCount = musicFiles.Count();
            var srcList = GetSrcList(musicFiles);

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

        private static string GetSrcList(IEnumerable<string> musicFiles)
        {
            var srcs = musicFiles
                .Select(x => $@"            <media src=""{x}""/>")
                .Select(x => x.Replace("&", "&amp;").Replace("'", "&apos;"));
            


            return string.Join(Environment.NewLine, srcs);
        }
    }
}