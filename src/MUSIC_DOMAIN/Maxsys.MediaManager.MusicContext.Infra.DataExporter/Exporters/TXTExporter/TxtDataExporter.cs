using FluentValidation.Results;
using Maxsys.MediaManager.MusicContext.Infra.DataEFCore.Context;
using Maxsys.MediaManager.MusicContext.Infra.DataExporter.Exporters;
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
    public sealed class TxtDataExporter : DataExporterBase<MusicAppContext>
    {
        public TxtDataExporter(ILogger<TxtDataExporter> logger, MusicAppContext context)
            : base(logger, context)
        { }

        public override async Task<ValidationResult> ExportAsync(string exportFolder)
        {
            var dbName = _context.Database.GetDbConnection().Database;
            var xlFile = $@"{exportFolder}\{dbName}_{DateTime.Now:yyyyMMddHHmmss}.txt";
            var contents = new List<string> { $"TESTE TxtDataExporter\nID\tCATALOGO" };

            var validationResult = new ValidationResult();

            #region Getting context data

            IEnumerable<string> catalogs = null;

            try
            {
                _logger.LogDebug("Retrieving data from database...");

                catalogs = await _context.MusicCatalogs
                    .AsNoTracking()
                    .Select(c => $"{c.Id}\t{c.Name}")
                    .ToListAsync();

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

                contents.AddRange(catalogs);

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
                validationResult = CreateExportFolder(Path.GetDirectoryName(xlFile));
                if (validationResult.IsValid)
                {
                    _logger.LogInformation("Exporting to file...");

                    await File.WriteAllLinesAsync(xlFile, contents, Encoding.Unicode);

                    _logger.LogInformation($"File exported to <{xlFile}>.");
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
    }
}