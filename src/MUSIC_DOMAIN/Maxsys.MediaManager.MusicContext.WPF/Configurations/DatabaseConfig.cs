using Maxsys.MediaManager.MusicContext.Infra.CrossCutting.IoC;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Maxsys.MediaManager.MusicContext.WPF.Configurations
{
    public static class DatabaseConfig
    {
        public static void AddDatabaseConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));
            if (configuration == null) throw new ArgumentNullException(nameof(configuration));

            // Config Identity here
            // ...

            NativeInjectorBootstrapper.RegisterDatabase(services, configuration);
        }
    }
}