//using Maxsys.MediaManager.MusicContext.Domain.Interfaces.Services;
//using Maxsys.MediaManager.MusicContext.Domain.Services.PlaylistExporters;
//using Microsoft.VisualStudio.TestTools.UnitTesting;
//using System.Collections.Generic;
//using System.IO;
//using System.Threading.Tasks;

//namespace Maxsys.MediaManager.MusicContext.Tests.PlaylistExportTests
//{
//    [TestClass]
//    [TestCategory("Domain - Services: " + nameof(IPlaylistFileExporter))]
//    public class PlaylistExportTest
//    {
//        private const string LIBRARY_FOLDER = @"D:\ARQUIVOS\Song";
//        private static void ClearTestPlaylist(string file)
//        {
//            if (File.Exists(file))
//                File.Delete(file);
//        }

//        [TestMethod]
//        public async Task M3UPlaylistFileExporter_Test()
//        {
//            #region Arrange

//            var destFolder = $@"{LIBRARY_FOLDER}\Playlists\";
//            var playlistName = "DevTestPlaylist";
//            var musicFiles = new List<string>(5)
//            {
//                $@"{LIBRARY_FOLDER}\Artist1\Song1.mp3",
//                $@"{LIBRARY_FOLDER}\Artist1\Song2.mp3",
//                $@"{LIBRARY_FOLDER}\Artist2\Song3.mp3",
//                $@"{LIBRARY_FOLDER}\Artist3\Song4.mp3",
//                $@"{LIBRARY_FOLDER}\Artist3\Album1\Song6.mp3"
//            };
//            var expectedPlaylistContents = @"#EXTM3U
//..\Artist1\Song1.mp3
//..\Artist1\Song2.mp3
//..\Artist2\Song3.mp3
//..\Artist3\Song4.mp3
//..\Artist3\Album1\Song6.mp3
//";
//            var exporter = new M3UPlaylistFileExporter();
//            var playlistPath = Path.Combine(destFolder, $"{playlistName}.m3u");
//            #endregion Arrange

//            #region Act

//            var validationResult = await exporter.ExportFileAsync(musicFiles, destFolder, playlistName);

//            #endregion Act

//            #region Assert
//            if (validationResult.IsValid)
//            {
//                var playlistContents = File.ReadAllText(playlistPath);
//                Assert.AreEqual(expectedPlaylistContents, playlistContents);
//            }
//            else
//            {
//                Assert.Fail($"{validationResult}");
//            }
//            #endregion

//            ClearTestPlaylist(playlistPath);
//        }
//        [TestMethod]
//        public async Task WPLPlaylistFileExporter_Test()
//        {
//            #region Arrange

//            var destFolder = $@"{LIBRARY_FOLDER}\Playlists\";
//            var playlistName = "DevTestPlaylist";
//            var musicFiles = new List<string>(5)
//            {
//                $@"{LIBRARY_FOLDER}\Artist1\Song1.mp3",
//                $@"{LIBRARY_FOLDER}\Artist1\Song2.mp3",
//                $@"{LIBRARY_FOLDER}\Artist2\Song3.mp3",
//                $@"{LIBRARY_FOLDER}\Artist3\Song4.mp3",
//                $@"{LIBRARY_FOLDER}\Artist3\Album1\Song6.mp3"
//            };
//            var expectedPlaylistContents = @"<?wpl version=""1.0""?>
//<smil>
//    <head>
//        <meta name=""Generator"" content=""Maxsys Media Manager""/>
//        <meta name=""ItemCount"" content=""5""/>
//        <title>DevTestPlaylist</title>
//    </head>
//    <body>
//        <seq>
//            <media src=""..\Artist1\Song1.mp3""/>
//            <media src=""..\Artist1\Song2.mp3""/>
//            <media src=""..\Artist2\Song3.mp3""/>
//            <media src=""..\Artist3\Song4.mp3""/>
//            <media src=""..\Artist3\Album1\Song6.mp3""/>
//        </seq>
//    </body>
//</smil>";
//            var exporter = new WMPPlaylistFileExporter();
//            var playlistPath = Path.Combine(destFolder, $"{playlistName}.wpl");
//            #endregion Arrange

//            #region Act

//            var validationResult = await exporter.ExportFileAsync(musicFiles, destFolder, playlistName);

//            #endregion Act

//            #region Assert
//            if (validationResult.IsValid)
//            {
//                var playlistContents = File.ReadAllText(playlistPath);
//                Assert.AreEqual(expectedPlaylistContents, playlistContents);
//            }
//            else
//            {
//                Assert.Fail($"{validationResult}");
//            }
//            #endregion

//            ClearTestPlaylist(playlistPath);
//        }
//    }
//}