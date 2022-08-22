using Maxsys.MediaManager.CoreDomain.Extensions;
using Maxsys.MediaManager.MusicContext.AppTagLibMono.Extensions;
using Maxsys.MediaManager.MusicContext.Domain.Extensions;
using Maxsys.MediaManager.MusicContext.Infra.DataEFCore.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Maxsys.MediaManager.MusicContext.Infra.CrossCutting.IoC
{
    public static class NativeInjectorBootstrapper
    {
        /// <summary>
        /// Register services using native Dependency Injection
        /// </summary>
        public static IServiceCollection RegisterServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddCoreDependencyInjection();
            services.AddDomainDependencyInjection(configuration);
            services.AddTagLibMonoDependencyInjection();
            services.AddDataEFCoreDependencyInjection(configuration);

            return services;
        }

        /// <summary>
        /// Register Logging using native Dependency Injection
        /// </summary>
        public static void RegisterLogging(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddLogging(configure => configure

                .ClearProviders()
                .AddConfiguration(configuration.GetSection("Logging"))
                .AddConsole()

                //.SetMinimumLevel(LogLevel.Trace)

                /* ConsoleLogger example
                .AddColorConsoleLogger(configuration =>
                {
                    configuration.LogLevel = LogLevel.Warning;
                    configuration.Color = ConsoleColor.DarkMagenta;
                })
                //*/// ================================================

                /* FileLogger example
                .AddFileLogger(options =>
                {
                    options.MaxFileSizeInMB = 5;
                    options.RetainPolicyFileCount = 10;
                    options.FileNamePrefix = "InfoLog";
                    options.LogLevel = LogLevel.Information;
                })

                .AddFileLogger(options =>
                {
                    options.MaxFileSizeInMB = 5;
                    options.RetainPolicyFileCount = 2;
                    options.FileNamePrefix = "TraceLog";
                    options.LogLevel = LogLevel.Debug;
                })

                //*/// ================================================
                );
        }
    }
}