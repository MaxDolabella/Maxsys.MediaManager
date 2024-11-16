using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation.Results;
using Maxsys.MediaManager.MusicContext.Infra.DataEFCore.Context;
using Maxsys.MediaManager.MusicContext.Infra.DataExporter.Exporters;
using Maxsys.MediaManager.MusicContext.Infra.DataExporter.Exporters.TXTExporter.DTOs;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Maxsys.MediaManager.MusicContext.Infra.DataExporter
{
    public sealed class TxtDataExporter : DataExporterBase<MusicAppContext>
    {
        public TxtDataExporter(ILogger<TxtDataExporter> logger, MusicAppContext context)
            : base(logger, context)
        { }

        public override async Task<ValidationResult> ExportAsync(string exportFolder)
        {
            var dbName = _context.Database.GetDbConnection().Database;
            var txtFile = $@"{exportFolder}\{dbName}_{DateTime.Now:yyyyMMddHHmmss}.txt";
            var header = GetHeader();

            var contents = new List<string> { header };

            var validationResult = new ValidationResult();

            #region Getting context data

            IReadOnlyCollection<SongTxtDTO> songs = null;

            try
            {
                _logger.LogDebug("Retrieving data from database...");

                songs = await GetDTOs(_context);

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

            #endregion Getting context data

            #region Adding to contents

            try
            {
                _logger.LogDebug("Creating contents...");

                contents.AddRange(songs.Select(song => GetLine(song)));

                _logger.LogDebug("Contents created.");
            }
            catch (Exception ex)
            {
                validationResult.AddException(ex, "Creating contents has failed");
                _logger.LogError($"Creating contents has failed: {ex}");
            }

            if (!validationResult.IsValid) return validationResult;

            #endregion Adding to contents

            #region Exporting to file

            try
            {
                validationResult = CreateExportFolder(Path.GetDirectoryName(txtFile));
                if (validationResult.IsValid)
                {
                    _logger.LogInformation("Exporting to file...");

                    await File.WriteAllLinesAsync(txtFile, contents, Encoding.Unicode);

                    _logger.LogInformation($"File exported to <{txtFile}>.");
                }
            }
            catch (Exception ex)
            {
                validationResult.AddException(ex, "Exporting to file has failed");
                _logger.LogError($"Exporting to file has failed: {ex}");
            }

            #endregion Exporting to file

            return validationResult;
        }

        private static async Task<IReadOnlyCollection<SongTxtDTO>> GetDTOs(MusicAppContext context)
        {
            var query = context.Songs
                    .AsNoTrackingWithIdentityResolution()
                    .Include(song => song.Album.Artist.Catalog)
                    .OrderBy(song => song.FullPath)
                    .Select(song => new SongTxtDTO(
                        song.Id,
                        song.FullPath,
                        song.TrackNumber,
                        song.Title,
                        song.Classification.Rating,
                        song.Classification.GetStars10(),
                        song.SongProperties.Duration,
                        song.AlbumId,
                        song.Album.Year,
                        song.Album.Name,
                        song.Album.Genre,
                        song.Album.ArtistId,
                        song.Album.Artist.Name,
                        song.Album.Artist.CatalogId,
                        song.Album.Artist.Catalog.Name));

            var songs = await query.ToListAsync();

            return songs;
        }

        private static string GetHeader()
        {
            var header = string.Join('\t', new List<string>
            {
                nameof(SongTxtDTO.SongId),
                nameof(SongTxtDTO.SongFullPath),
                nameof(SongTxtDTO.SongNumber),
                nameof(SongTxtDTO.SongTitle),
                nameof(SongTxtDTO.SongRating),
                nameof(SongTxtDTO.SongStars10),
                nameof(SongTxtDTO.SongDuration),
                nameof(SongTxtDTO.AlbumId),
                nameof(SongTxtDTO.AlbumYear),
                nameof(SongTxtDTO.AlbumName),
                nameof(SongTxtDTO.AlbumGenre),
                nameof(SongTxtDTO.ArtistId),
                nameof(SongTxtDTO.ArtistName),
                nameof(SongTxtDTO.CatalogId),
                nameof(SongTxtDTO.CatalogName)
            });

            return header;
        }

        private static string GetLine(SongTxtDTO song)
        {
            var line = string.Join('\t', new List<string>
            {
                $"{song.SongId}",
                $"{song.SongFullPath}",
                $"{song.SongNumber}",
                $"{song.SongTitle}",
                $"{song.SongRating}",
                $"{song.SongStars10}",
                $"{song.SongDuration:hh\\:mm\\:ss}",
                $"{song.AlbumId}",
                $"{song.AlbumYear}",
                $"{song.AlbumName}",
                $"{song.AlbumGenre}",
                $"{song.ArtistId}",
                $"{song.ArtistName}",
                $"{song.CatalogId}",
                $"{song.CatalogName}"
            });

            return line;
        }
    }
}