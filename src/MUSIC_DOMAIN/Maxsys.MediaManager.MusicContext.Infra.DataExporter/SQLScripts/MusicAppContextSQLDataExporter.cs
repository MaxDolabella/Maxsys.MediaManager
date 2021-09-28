using Maxsys.MediaManager.MusicContext.Infra.DataEFCore.Context;
using Maxsys.MediaManager.MusicContext.Infra.DataExporter.SQLValuesScript.Scripts;
using Maxsys.MediaManager.MusicContext.Infra.DataExporter.SQLValuesScript.Scripts.Base;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Maxsys.MediaManager.MusicContext.Infra.DataExporter
{
    public class MusicAppContextSQLDataExporter : IDataExporter
    {
        private readonly MusicAppContext _context;
        public MusicAppContextSQLDataExporter(MusicAppContext context)
        {
            _context = context;
        }

        public void Export()
        {
            var dbName = _context.Database.GetDbConnection().Database;
            var desktopFolder = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            var sqlFile = $@"{desktopFolder}\{dbName}_{DateTime.Now:yyyyMMddHHmmss}.sql";
            var contents = new List<string> { $"USE [{dbName}]\nGO" };

            Console.WriteLine("Retrieving data from database...");
            
            #region Context
            var catalogs = _context.MusicCatalogs.ToList();
            var artists = _context.Artists.ToList();
            var albums = _context.Albums.ToList();
            var musics = _context.Musics.ToList();
            var composers = _context.Composers.ToList();
            var playlists = _context.Playlists.ToList();

            var composerMusic = _context.Musics
                .Where(m => m.Composers.Count > 0)
                .ToList()
                .SelectMany(m => m.Composers.Select(c => (c, m)));

            var playlistItems = playlists.SelectMany(p => p.Items);

            _context.Dispose();
            #endregion

            Console.WriteLine("Data retrieved from database.");

            Console.WriteLine("Creating scripts...");

            contents.AddRange(GetValues(catalogs, new MusicCatalogScripts()));
            contents.AddRange(GetValues(artists, new ArtistScripts()));
            contents.AddRange(GetValues(albums, new AlbumScripts()));
            
            contents.AddRange(GetValues(composers, new ComposerScripts()));
            contents.AddRange(GetValues(musics, new MusicScripts()));
            contents.AddRange(GetValues(composerMusic, new ComposerMusicScripts()));

            contents.AddRange(GetValues(playlists, new PlaylistScripts()));
            contents.AddRange(GetValues(playlistItems, new PlaylistItemScripts()));

            Console.WriteLine("Scripts created.");

            Console.WriteLine("Exporting to file...");

            File.WriteAllLines(sqlFile, contents, Encoding.Unicode);
            
            Console.WriteLine($"Scripts exported to file <{sqlFile}>.");
        }

        
        static IEnumerable<string> GetValues<T>(IEnumerable<T> list, InsertScriptBase<T> insertScript)
        {
            var values = list.Select(obj => insertScript.GetInsertValues(obj)).ToList();

            for (int i = 0; i < values.Count; i++)
            {
                if (i % (1000 + 1) == 0)
                {
                    values.Insert(i, insertScript.InsertScript);
                    if (i > 1) values[i - 1] = TrimLast(values[i - 1]);
                }
            }

            if (values.Count > 0) values[^1] = TrimLast(values[^1]); // [^1] = [string.Count - 1]

            return values;
        }

        static string TrimLast(string text) => text[0..^1]; // [0..^1] = Substring(0, values[i].Length - 1);
    }
}
