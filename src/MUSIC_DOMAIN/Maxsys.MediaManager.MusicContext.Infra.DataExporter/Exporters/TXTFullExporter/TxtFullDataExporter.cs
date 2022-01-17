using FluentValidation.Results;
using Maxsys.MediaManager.MusicContext.Infra.DataEFCore.Context;
using Maxsys.MediaManager.MusicContext.Infra.DataExporter.Exporters;
using Maxsys.MediaManager.MusicContext.Infra.DataExporter.Exporters.TXTFullExporter.DTOs;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Maxsys.MediaManager.MusicContext.Infra.DataExporter
{
    public sealed class TxtFullDataExporter : DataExporterBase<MusicAppContext>
    {
        public TxtFullDataExporter(ILogger<TxtFullDataExporter> logger, MusicAppContext context)
            : base(logger, context)
        { }

        public override async Task<ValidationResult> ExportAsync(string exportFolder)
        {
            var dbName = _context.Database.GetDbConnection().Database;
            var txtFile = $@"{exportFolder}\{dbName}_FULL_{DateTime.Now:yyyyMMddHHmmss}.txt";
            var header = GetHeader();

            var contents = new List<string> { header };

            var validationResult = new ValidationResult();

            #region Getting context data

            IReadOnlyCollection<SongTxtFullDTO> songs = null;

            try
            {
                _logger.LogDebug("Retrieving data from database...");

                songs = await GetDTOs(_context);

                _logger.LogDebug("Data retrieved from database.");
            }
            catch (Exception ex)
            {
                validationResult.AddFailure(ex);
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
                validationResult.AddFailure(ex);
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
                validationResult.AddFailure(ex);
                _logger.LogError($"Exporting to file has failed: {ex}");
            }

            #endregion Exporting to file

            return validationResult;
        }

        private static async Task<IReadOnlyCollection<SongTxtFullDTO>> GetDTOs(MusicAppContext context)
        {
            var query = context.Musics
                    .AsNoTrackingWithIdentityResolution()
                    .Include(song => song.Album.Artist.MusicCatalog)
                    .OrderBy(song => song.FullPath)
                    .Select(song => new SongTxtFullDTO(
                        song.Id,
                        song.FullPath,
                        song.TrackNumber,
                        song.Title,
                        song.Comments,
                        song.MusicDetails.IsBonusTrack,
                        song.MusicDetails.VocalGender,
                        song.MusicDetails.CoveredArtist,
                        song.MusicDetails.FeaturedArtist,
                        song.Classification.Rating,
                        song.Classification.GetStars10(),
                        song.MusicProperties.Duration,
                        song.MusicProperties.BitRate,
                        song.AlbumId,
                        song.Album.Year,
                        song.Album.Name,
                        song.Album.Genre,
                        song.Album.AlbumType,
                        song.Album.ArtistId,
                        song.Album.Artist.Name,
                        song.Album.Artist.MusicCatalogId,
                        song.Album.Artist.MusicCatalog.Name));

            var songs = await query.ToListAsync();

            return songs;
        }

        private static string GetHeader()
        {
            var header = string.Join('\t', new List<string>
            {
                nameof(SongTxtFullDTO.SongId),
                nameof(SongTxtFullDTO.SongFullPath),
                nameof(SongTxtFullDTO.SongNumber),
                nameof(SongTxtFullDTO.SongTitle),
                nameof(SongTxtFullDTO.SongComments),
                nameof(SongTxtFullDTO.SongIsBonusTrack),
                nameof(SongTxtFullDTO.SongVocalGender),
                nameof(SongTxtFullDTO.SongCoveredArtist),
                nameof(SongTxtFullDTO.SongFeaturedArtist),
                nameof(SongTxtFullDTO.SongRating),
                nameof(SongTxtFullDTO.SongStars10),
                nameof(SongTxtFullDTO.SongDuration),
                nameof(SongTxtFullDTO.SongBitRate),
                nameof(SongTxtFullDTO.AlbumId),
                nameof(SongTxtFullDTO.AlbumYear),
                nameof(SongTxtFullDTO.AlbumName),
                nameof(SongTxtFullDTO.AlbumGenre),
                nameof(SongTxtFullDTO.AlbumType),
                nameof(SongTxtFullDTO.ArtistId),
                nameof(SongTxtFullDTO.ArtistName),
                nameof(SongTxtFullDTO.CatalogId),
                nameof(SongTxtFullDTO.CatalogName)
            });

            return header;
        }

        private static string GetLine(SongTxtFullDTO song)
        {
            var line = string.Join('\t', new List<string>
            {
                $"{song.SongId}",
                $"{song.SongFullPath}",
                $"{song.SongNumber}",
                $"{song.SongTitle}",
                $"{song.SongComments}",
                $"{song.SongIsBonusTrack}",
                $"{song.SongVocalGender}",
                $"{song.SongCoveredArtist}",
                $"{song.SongFeaturedArtist}",
                $"{song.SongRating}",
                $"{song.SongStars10}",
                $"{song.SongDuration:hh\\:mm\\:ss}",
                $"{song.SongBitRate}",
                $"{song.AlbumId}",
                $"{song.AlbumYear}",
                $"{song.AlbumName}",
                $"{song.AlbumGenre}",
                $"{song.AlbumType}",
                $"{song.ArtistId}",
                $"{song.ArtistName}",
                $"{song.CatalogId}",
                $"{song.CatalogName}"
            });

            return line;
        }
    }
}