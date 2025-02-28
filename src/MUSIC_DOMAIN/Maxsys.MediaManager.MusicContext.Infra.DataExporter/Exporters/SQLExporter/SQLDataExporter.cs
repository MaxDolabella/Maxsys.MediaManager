using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation.Results;
using Maxsys.MediaManager.MusicContext.Domain.Entities;
using Maxsys.MediaManager.MusicContext.Infra.DataEFCore.Context;
using Maxsys.MediaManager.MusicContext.Infra.DataExporter.Exporters;
using Maxsys.MediaManager.MusicContext.Infra.DataExporter.SQLValuesScript.Scripts;
using Maxsys.MediaManager.MusicContext.Infra.DataExporter.SQLValuesScript.Scripts.Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Maxsys.MediaManager.MusicContext.Infra.DataExporter
{
    public sealed class SQLDataExporter : DataExporterBase<MusicAppContext>
    {
        public SQLDataExporter(ILogger<SQLDataExporter> logger, MusicAppContext context)
            : base(logger, context)
        { }

        public override async Task<ValidationResult> ExportAsync(string exportFolder)
        {
            var dbName = _context.Database.GetDbConnection().Database;
            var sqlFile = $@"{exportFolder}\{dbName}_{DateTime.Now:yyyyMMddHHmmss}.sql";
            var contents = new List<string> { $"USE [{dbName}]\nGO" };

            var validationResult = new ValidationResult();

            #region Getting data context

            IEnumerable<Catalog> catalogs = null;
            IEnumerable<Artist> artists = null;
            IEnumerable<Album> albums = null;
            IEnumerable<Song> musics = null;
            IEnumerable<Composer> composers = null;
            IEnumerable<Playlist> playlists = null;
            IEnumerable<(Composer c, Song m)> composerMusic = null;
            IEnumerable<PlaylistItem> playlistItems = null;

            try
            {
                _logger.LogDebug("Retrieving data from database...");

                catalogs = await _context.Set<Catalog>().AsNoTracking().ToListAsync();
                artists = await _context.Set<Artist>().AsNoTracking().ToListAsync();
                albums = await _context.Set<Album>().AsNoTracking().ToListAsync();
                musics = await _context.Set<Song>().Include(m => m.Composers).AsNoTracking().ToListAsync();
                composers = await _context.Set<Composer>().AsNoTracking().ToListAsync();
                playlists = await _context.Set<Playlist>().AsNoTracking().ToListAsync();

                composerMusic = musics
                    .Where(m => m.Composers.Any())
                    .SelectMany(m => m.Composers.Select(c => (c, m)))
                    .ToList();

                playlistItems = playlists.SelectMany(p => p.Items);

                _logger.LogDebug("Data retrieved from database.");
            }
            catch (Exception ex)
            {
                validationResult.AddException(ex, "Retrieving data from database has failed");
                _logger.LogError($"Retrieving data from database has failed: {ex}");
            }
            finally
            {
                _context.Dispose();
            }

            if (!validationResult.IsValid) return validationResult;

            #endregion Getting data context

            #region Adding scripts to contents

            try
            {
                _logger.LogDebug("Creating scripts...");

                contents.AddRange(GetValues(catalogs, new MusicCatalogScripts()));
                contents.AddRange(GetValues(artists, new ArtistScripts()));
                contents.AddRange(GetValues(albums, new AlbumScripts()));

                contents.AddRange(GetValues(composers, new ComposerScripts()));
                contents.AddRange(GetValues(musics, new MusicScripts()));
                contents.AddRange(GetValues(composerMusic, new ComposerMusicScripts()));

                contents.AddRange(GetValues(playlists, new PlaylistScripts()));
                contents.AddRange(GetValues(playlistItems, new PlaylistItemScripts()));

                _logger.LogDebug("Scripts created.");
            }
            catch (Exception ex)
            {
                validationResult.AddException(ex, "Creating scripts has failed");
                _logger.LogError($"Creating scripts has failed: {ex}");
            }

            if (!validationResult.IsValid) return validationResult;

            #endregion Adding scripts to contents

            #region Exporting to file

            try
            {
                validationResult = CreateExportFolder(Path.GetDirectoryName(sqlFile));
                if (validationResult.IsValid)
                {
                    _logger.LogInformation("Exporting to file...");

                    await File.WriteAllLinesAsync(sqlFile, contents, Encoding.Unicode);

                    _logger.LogInformation($"Scripts exported to file <{sqlFile}>.");
                }
            }
            catch (Exception ex)
            {
                validationResult.AddException(ex, "Export to file has failed");
                _logger.LogError($"Export to file has failed: {ex}");
            }

            #endregion Exporting to file

            return validationResult;
        }

        private static IEnumerable<string> GetValues<T>(IEnumerable<T> list, InsertScriptBase<T> insertScript)
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

        private static string TrimLast(string text) => text[0..^1]; // [0..^1] = Substring(0, values[i].Length - 1);
    }
}