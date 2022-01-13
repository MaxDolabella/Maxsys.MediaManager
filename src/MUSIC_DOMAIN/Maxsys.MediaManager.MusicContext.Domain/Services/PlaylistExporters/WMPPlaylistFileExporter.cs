using FluentValidation.Results;
using Maxsys.MediaManager.MusicContext.Domain.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Maxsys.MediaManager.MusicContext.Domain.Services.PlaylistExporters
{
    public sealed class WMPPlaylistFileExporter : IPlaylistFileExporter
    {
        // WARNING ValueTask ?
        public async Task<ValidationResult> ExportFile(IEnumerable<string> musicFiles, string destFolder, string playlistName)
        {
            var path = Path.Combine(destFolder, $"{playlistName}.wpl");
            var contents = GetContents(musicFiles, playlistName);

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
</smil>
";
        }

        private static string GetSrcList(IEnumerable<string> musicFiles)
        {
            // WARNING for now, use absolute path. Later, use relative path.

            //src => <media src="..\ROCK\Avantasia\Studio\(2016) Ghostlights\09 Babylon Vampyres.mp3"/>
            var srcs = musicFiles
                .Select(x => $@"<media src=""{x}""/>");

            return string.Join("\t\t\t\n", srcs);
        }
    }
}

/*

<?wpl version="1.0"?>
<smil>
    <head>
        <meta name="Generator" content="Maxsys Media Manager"/>
        <meta name="ItemCount" content="8"/>
        <title>Test</title>
    </head>
    <body>
        <seq>
            <media src="..\ROCK\Avantasia\Studio\(2019) Moonglow\08 The Piper at the Gates of Dawn.mp3"/>
            <media src="..\ROCK\Avantasia\Studio\(2016) Ghostlights\09 Babylon Vampyres.mp3"/>
            <media src="..\ROCK\Bruce Dickinson\Live\(1999) Scream For Me Brazil\06 Book Of Thel.mp3"/>
            <media src="..\ROCK\Bruce Dickinson\Studio\(1997) Accident Of Birth\10 Welcome To The Pit.mp3"/>
            <media src="..\ROCK\HammerFall\Diversos - HammerFall\Riders Of The Storm.mp3"/>
            <media src="..\ROCK\Hangar\Studio\(2007) The Reason Of Your Conviction\03 Hastiness.mp3"/>
            <media src="..\ROCK\New Years Day\Diversos - New Years Day\Kill or Be Killed.mp3"/>
            <media src="..\ROCK\Shadowside\Studio\(2017) Shades of Humanity\08 Parade The Sacrifice.mp3"/>
        </seq>
    </body>
</smil>

*/