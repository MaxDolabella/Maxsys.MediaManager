using FluentValidation.Results;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Maxsys.MediaManager.MusicContext.Infra.DataExporter.Exporters
{
    public abstract class DataExporterBase<TContext> : IDataExporter
        where TContext : DbContext
    {
        protected readonly ILogger _logger;
        protected readonly TContext _context;

        protected DataExporterBase(ILogger logger, TContext context)
        {
            _logger = logger;
            _context = context;
        }

        /// <summary>
        /// Asynchronously exports the data in the context
        /// </summary>
        /// <returns></returns>
        public abstract Task<ValidationResult> ExportAsync(string exportFolder);

        /// <summary>
        /// Creates the folder where exported data will be placed.
        /// </summary>
        /// <param name="exportFolder">folder path where exported data will be placed</param>
        /// <returns>a valid <see cref="ValidationResult"/> if the folder was created,
        /// or invalid with the failure when creating it.</returns>
        protected ValidationResult CreateExportFolder(string exportFolder)
        {
            var validationResult = new ValidationResult();

            try
            {
                _logger.LogInformation($"Creating export folder <{exportFolder}>...");

                _ = Directory.CreateDirectory(exportFolder);

                _logger.LogInformation("Export folder created.");
            }
            catch (Exception ex)
            {
                validationResult.AddFailure(ex);
                _logger.LogError($"Export folder creation has failed: {ex}");
            }

            return validationResult;
        }
    }
}