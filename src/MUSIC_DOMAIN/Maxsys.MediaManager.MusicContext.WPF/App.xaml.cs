using Maxsys.MediaManager.MusicContext.Infra.CrossCutting.IoC;
using Maxsys.MediaManager.MusicContext.WPF.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Reflection;
using System.Windows;

namespace Maxsys.MediaManager.MusicContext.WPF
{
    // Notes: Understanding .NET Generic Host Model: https://sahansera.dev/dotnet-core-generic-host/
    public partial class App : Application
    {
        private readonly IHost _host;

        public static string AppVersion { get; } = Assembly.GetEntryAssembly()
            .GetCustomAttribute<AssemblyInformationalVersionAttribute>().InformationalVersion;

        public App()
        {
            _host = Host.CreateDefaultBuilder()
                .ConfigureServices((context, services) =>
                {
                    //NativeInjectorBootstrapper.RegisterServices(services, context.Configuration);

                    IoCExtensions.AddUIServices(services, context.Configuration);
                })
                .Build();
        }

        protected override async void OnStartup(StartupEventArgs e)
        {
            using (var scope = _host.Services.CreateScope())
            {
                var logger = scope.ServiceProvider.GetRequiredService<ILogger<App>>();
                var config = scope.ServiceProvider.GetRequiredService<IConfiguration>();

                logger.LogWarning("AppVersion = [{AppVersion}]", AppVersion);
                logger.LogWarning("Config:ENVIRONMENT = [{config}]", config["ENVIRONMENT"]);
                logger.LogWarning("Config:DOTNET_ENVIRONMENT = [{config}]", config["DOTNET_ENVIRONMENT"]);
            }

            // await _host.CreateDatabaseIfNotExistAndApplyMigrationsAsync();

            await _host.StartAsync();

            Current.MainWindow = _host.Services.GetRequiredService<MainWindow>();
            Current.MainWindow.Show();

            base.OnStartup(e);
        }

        protected override async void OnExit(ExitEventArgs e)
        {
            using (_host)
            {
                await _host.StopAsync();
            }

            base.OnExit(e);
        }
    }
}