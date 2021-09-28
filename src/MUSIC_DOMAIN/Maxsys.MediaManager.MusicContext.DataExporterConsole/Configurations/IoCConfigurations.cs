using Maxsys.MediaManager.MusicContext.Infra.CrossCutting.IoC;
using Maxsys.MediaManager.MusicContext.Infra.DataExporter;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Maxsys.MediaManager.MusicContext.DataExporterConsole.Configurations
{
    internal static class IoCConfigurations
    {
        internal static void AddConfigurations(this IServiceCollection services, IConfiguration configuration)
        {
            NativeInjectorBootstrapper.RegisterOptions(services, configuration);
            NativeInjectorBootstrapper.RegisterLogging(services, configuration);
            NativeInjectorBootstrapper.RegisterDatabase(services, configuration);

            services.AddTransient<MusicAppContextSQLDataExporter>();
        }
    }
}