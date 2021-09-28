using CommandLine;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Maxsys.MediaManager.MusicContext.DataExporterConsole.Commands
{
    abstract class ConfigurationReadableCommand : ICommand
    {
        private readonly IConfiguration _configuration;

        public ConfigurationReadableCommand(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public abstract void Execute();
    }

    [Verb("export", HelpText = "Export the database.")]
    class ExportDatabaseCommand : ConfigurationReadableCommand
    {
        public ExportDatabaseCommand(IConfiguration configuration)
            : base(configuration)
        { }

        [Option('c', "context", Required = true, HelpText = "Is the context to export. Ex: 'MusicAppContext'")]
        public string Context { get; set; }

        public override void Execute()
        {

        }
    }
}
