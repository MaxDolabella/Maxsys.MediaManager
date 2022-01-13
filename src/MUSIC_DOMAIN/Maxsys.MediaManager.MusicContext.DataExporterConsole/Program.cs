using CommandLine;
using Maxsys.MediaManager.MusicContext.DataExporterConsole.Commands;
using Maxsys.MediaManager.MusicContext.DataExporterConsole.Configurations;
using Maxsys.MediaManager.MusicContext.Infra.DataExporter.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Threading.Tasks;

namespace Maxsys.MediaManager.MusicContext.DataExporterConsole
{
    //https://makolyte.com/csharp-parsing-commands-and-arguments-in-a-console-app/
    //https://github.com/commandlineparser/commandline/wiki
    internal class Program
    {
        private static readonly IHost _host;

        static Program()
        {
            Console.Title = $"Data Exporter";

            _host = Host.CreateDefaultBuilder()
                .ConfigureServices((context, services) =>
                {
                    DomainPropertiesConfigurations.SetDomainProperties(context.Configuration);
                    IoCConfigurations.AddConfigurations(services, context.Configuration);
                    services.ConfigureDataExporters();
                })
                .Build();
        }

        private static async Task Main(string[] args)
        {
            using (var serviceScope = _host.Services.CreateScope())
            {
                var serviceProvider = serviceScope.ServiceProvider;

                var result = await Parser.Default
                    .ParseArguments(()
                        => new ExportDatabaseCommandAsync(serviceProvider), args)
                    .WithParsedAsync(e => e.ExecuteAsync());
            }

            ShowPressKeyToExit();
        }

        private static void ShowPressKeyToExit()
        {
            #region CONSOLE APP FIM

            var corPadrao = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine();
            Console.WriteLine("CONSOLE APP END:");
            Console.WriteLine("Press any key to exit...");
            Console.ForegroundColor = corPadrao;
            Console.ReadKey();

            #endregion CONSOLE APP FIM
        }
    }
}