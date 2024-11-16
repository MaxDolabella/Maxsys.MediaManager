using Maxsys.MediaManager.MusicContext.Infra.CrossCutting.IoC;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Maxsys.MediaManager.MusicContext.DataExporterConsole.Configurations;

internal static class IoCExtensions
{
    internal static void AddConfigurations(this IServiceCollection services, IConfiguration configuration)
    {
        NativeInjectorBootstrapper.RegisterServices(services, configuration);
        NativeInjectorBootstrapper.RegisterLogging(services, configuration);
    }
}