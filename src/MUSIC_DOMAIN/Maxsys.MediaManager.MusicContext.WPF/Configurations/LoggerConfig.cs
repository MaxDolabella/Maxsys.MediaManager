using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;
using System;

namespace Maxsys.MediaManager.MusicContext.WPF.Configurations
{
    public static class LoggerConfig
    {
        public static void AddLoggerConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));
            if (configuration == null) throw new ArgumentNullException(nameof(configuration));

            #region Serilog configuration

            var outputTemplate = "{Timestamp:yyyy-MM-dd HH:mm:ss.fff}|{Level}|{Message:l}{NewLine}{Exception}";
            var fileSize_1MB = 1048576L;
            var retainedFileCountLimit = 2;

            var dbgSerilogLogger = new LoggerConfiguration()
                .WriteTo.File(
                    path: "Logs\\MediaManager-Debug.log",
                    restrictedToMinimumLevel: Serilog.Events.LogEventLevel.Debug,
                    outputTemplate: outputTemplate,
                    fileSizeLimitBytes: fileSize_1MB,
                    retainedFileCountLimit: retainedFileCountLimit)
                .CreateLogger();

            var infoSerilogLogger = new LoggerConfiguration()
                .WriteTo.File(
                    path: "Logs\\MediaManager.log",
                    restrictedToMinimumLevel: Serilog.Events.LogEventLevel.Information,
                    outputTemplate: outputTemplate,
                    fileSizeLimitBytes: fileSize_1MB,
                    retainedFileCountLimit: retainedFileCountLimit)
                .MinimumLevel.Override("Microsoft.EntityFrameworkCore", Serilog.Events.LogEventLevel.Warning)
                .CreateLogger();

            #endregion Serilog configuration

            services.AddLogging(builder => builder
                .ClearProviders()
                .AddConfiguration(configuration.GetSection("Logging"))
                .AddSimpleConsole()
                .AddSerilog(logger: dbgSerilogLogger, dispose: true)
                .AddSerilog(logger: infoSerilogLogger, dispose: true)
                );
        }
    }
}