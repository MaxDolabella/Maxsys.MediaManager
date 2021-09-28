using Maxsys.MediaManager.MusicContext.Infra.DataExporter;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;

namespace Maxsys.MediaManager.MusicContext.DataExporterConsole.Exporters
{
    public static class MusicContextAsSQLInsertScriptsExporter
    {
        // TODO async
        public static void Export(IHost host)
        {
            try
            {
                Console.WriteLine("Exporting MusicContext as 'SQL Insert Scripts'...");

                var sqlExporter = host.Services.GetRequiredService<MusicAppContextSQLDataExporter>();

                sqlExporter.Export();

                Console.WriteLine("MusicContext exported as 'SQL Insert Scripts'.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error exporting as 'SQL Insert Scripts':\n{ex.Message}");
            }
        }
    }
}