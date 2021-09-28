using CommandLine;
using Maxsys.MediaManager.MusicContext.DataExporterConsole.Configurations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Windows.Input;

namespace Maxsys.MediaManager.MusicContext.DataExporterConsole
{
    //https://makolyte.com/csharp-parsing-commands-and-arguments-in-a-console-app/
    internal class Program
    {
        private static readonly IHost _host;

        static Program()
        {
            Console.Title = $"DataExporter";

            _host = Host.CreateDefaultBuilder()
                .ConfigureServices((context, services) =>
                {
                    DomainPropertiesConfigurations.SetDomainProperties(context.Configuration);
                    IoCConfigurations.AddConfigurations(services, context.Configuration);
                })
                .Build();
        }

        private static void Main(string[] args)
        {
            Parser.Default.ParseArguments<ICommand>(args)
                .WithParsed<ICommand>(t => t.Execute(null));

            try
            {
                //MusicContextAsSQLInsertScriptsExporter.Export(_host);

                var logger = _host.Services.GetRequiredService<ILogger<Program>>();
                var config = _host.Services.GetRequiredService<IConfiguration>();

                
            }
            catch (Exception ex)
            {
                Console.WriteLine($"FATAL ERROR: {ex.Message}");
            }
            finally
            {
                ShowPressKeyToExit();
            }
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

        /// <summary>
        /// Get the DOTNET_ENVIRONMENT environment variable. If not exists, is set to "Production".
        /// </summary>
        /// <returns>DOTNET_ENVIRONMENT variable value.</returns>
        private static string GetEnvironment()
        {
            var DOTNET_ENVIRONMENT = Environment.GetEnvironmentVariable("DOTNET_ENVIRONMENT");

            if (string.IsNullOrWhiteSpace(DOTNET_ENVIRONMENT))
                Environment.SetEnvironmentVariable("DOTNET_ENVIRONMENT", Environments.Production);

            return DOTNET_ENVIRONMENT;
        }
    }
}