using Maxsys.MediaManager.MusicContext.Domain.Options;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Maxsys.MediaManager.MusicContext.WPF.Configurations
{
    public static class SettingsConfig
    {
        public static void AddSettingsConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));
            if (configuration == null) throw new ArgumentNullException(nameof(configuration));

            services.AddOptions();
            services.Configure<MusicSettings>(configuration.GetSection(MusicSettings.Section));

            //services.ConfigureOptions<>
        }
    }
}