using CommandLine;
using Maxsys.MediaManager.MusicContext.Infra.DataExporter;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;

namespace Maxsys.MediaManager.MusicContext.DataExporterConsole.Commands
{
    [Verb("export", HelpText = "Export the database.")]
    internal class ExportDatabaseCommandAsync : ICommandAsync
    {
        #region Fields

        private readonly IServiceProvider _serviceProvider;
        private readonly string _defaultOutputFolder;

        #endregion Fields

        #region Consts

        private const string FORMAT_OPTION_HELP_TEXT = @"Is the option format to export the database.
Options are 'txt', 'excel' and 'sql'";

        private const string OUTPUT_FOLDER_HELP_TEXT = "Is the folder where data will be exported.";

        #endregion Consts

        #region Properties

        [Option('f', "format", Required = true, HelpText = FORMAT_OPTION_HELP_TEXT)]
        public Format Format { get; set; }

        [Option('o', "output", Required = false, HelpText = OUTPUT_FOLDER_HELP_TEXT)]
        public string OutputFolder { get; set; } = null;

        #endregion Properties

        #region Ctor

        public ExportDatabaseCommandAsync(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;

            var configuration = serviceProvider.GetRequiredService<IConfiguration>();

            _defaultOutputFolder = configuration["CoreSettings:ExportedDataFolder"]
                ?? Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        }

        #endregion Ctor

        #region ICommandAsync

        public async Task ExecuteAsync()
        {
            IDataExporter dataExporter = GetDataExporterByFormatOption();

            if (dataExporter == default)
            {
                Console.Error.WriteLine($"{nameof(Format)} is invalid.");
            }
            else
            {
                var outputFolder = OutputFolder ?? _defaultOutputFolder;

                var validationResult = await dataExporter.ExportAsync(outputFolder);

                if (validationResult.IsValid)
                {
                    Console.WriteLine($"Data exported to <{outputFolder}>");
                }
                else
                {
                    Console.Error.WriteLine($"Error exporting data: {validationResult}");
                }
            }

            await Task.CompletedTask;

            return;
        }

        #endregion ICommandAsync

        private IDataExporter GetDataExporterByFormatOption()
        {
            return Format switch
            {
                Format.txt => _serviceProvider.GetService<TxtDataExporter>(),
                Format.excel => _serviceProvider.GetService<ExcelDataExporter>(),
                Format.sql => _serviceProvider.GetService<SQLDataExporter>(),
                _ => default
            };
        }
    }

    internal enum Format
    {
        txt, excel, sql, nn
    }
}