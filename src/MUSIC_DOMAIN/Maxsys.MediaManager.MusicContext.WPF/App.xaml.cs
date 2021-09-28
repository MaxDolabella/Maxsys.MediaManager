using Maxsys.MediaManager.MusicContext.WPF.Configurations;
using Maxsys.MediaManager.MusicContext.WPF.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Reflection;
using System.Windows;

namespace Maxsys.MediaManager.MusicContext.WPF
{
    /* Notes:
    Understanding .NET Generic Host Model: https://sahansera.dev/dotnet-core-generic-host/
    */

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
                    ConfigureDomainProperties(context.Configuration);
                    ConfigureServices(services, context.Configuration);
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
                logger.LogWarning("Mode = [{config}]", config["Mode"]);
            }

            await _host.CreateDatabaseIfNotExistAndApplyMigrationsAsync();

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

        private static void ConfigureDomainProperties(IConfiguration configuration)
        {
            AppDomain.CurrentDomain.SetData("DataDirectory", configuration["CoreSettings:DataDirectory"]);
        }

        private void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddSettingsConfiguration(configuration);

            services.AddLoggerConfiguration(configuration);

            services.AddDatabaseConfiguration(configuration);

            services.AddDependencyInjectionConfiguration();

            services.AddMVVMConfiguration();
        }
    }

    /*
    public partial class App : Application
    {
        public IServiceProvider ServiceProvider { get; private set; }
        public IConfiguration Configuration { get; private set; }

        // IConfiguration
        private void ConfigureConfiguration()
        {
            var jsonFile = "appsettings.json";
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile(jsonFile, optional: false, reloadOnChange: true);

            Configuration = builder.Build();
        }

        // Domain Properties
        private void ConfigureDomainProperties()
        {
            //var exePath = System.Reflection.Assembly.GetExecutingAssembly().Location;
            //var exeDir = System.IO.Path.GetDirectoryName(exePath);
            //AppDomain.CurrentDomain.SetData("DataDirectory", exeDir);

            AppDomain.CurrentDomain.SetData("DataDirectory", Configuration["CoreSettings:DataDirectory"]);
        }
    */
}