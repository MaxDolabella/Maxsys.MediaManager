using Maxsys.MediaManager.CoreDomain.Interfaces;
using Maxsys.MediaManager.MusicContext.ApplicationMVVM.Store;
using Maxsys.MediaManager.MusicContext.ApplicationMVVM.ViewModels;
using Maxsys.MediaManager.MusicContext.WPF.Commands;
using Maxsys.MediaManager.MusicContext.WPF.Commands.MainWindow;
using Maxsys.MediaManager.MusicContext.WPF.Services;
using Maxsys.ModelCore.Interfaces.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;
using System;
using System.Linq;

namespace Maxsys.MediaManager.MusicContext.WPF.Extensions;

public static class IoCExtensions
{
    internal static IServiceCollection AddUIServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.ConfigureLogger(configuration);

        // AppServices
        //services.ConfigureAppServicesByReflection();

        // Views
        //services.ConfigureViewsByReflection();

        // Others services
        //services.ConfigureMainViewServices();

        // Commands
        //services.ConfigureCommands();

        // NavigationStore
        services.AddSingleton<NavigationStore>();
        //services.AddSingleton<MainWindowViewModel>();

        //services.AddSingleton<IMainContentCloser, MainWindowContentCloser>();

        //services.AddSingleton<IMainContentOwner, MainWindowContentOwner>();
        //services.AddSingleton<IAppCloser, MainWindowAppCloser>();

        // Main Window as Singleton
        //services.AddSingleton<MainWindow>();

        return services;
    }

    internal static IServiceCollection ConfigureLogger(this IServiceCollection services, IConfiguration configuration)
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

        return services;
    }

    private static IServiceCollection ConfigureAppServicesByReflection(this IServiceCollection services)
    {
        var interfacesNamespace = "Maxsys.MediaManager.MusicContext.ApplicationMVVM.Interfaces.Services";
        var servicesNamespace = "Maxsys.MediaManager.MusicContext.ApplicationMVVM.Services";

        var assembly = System.Reflection.Assembly.Load("Maxsys.MediaManager.Music_App");
        var types = assembly.GetTypes();

        var iAppServices = types.Where(t => t.Namespace == interfacesNamespace && t.IsInterface).ToList();
        var appServices = types.Where(t => t.Namespace == servicesNamespace && t.Name.EndsWith("AppService")).ToList();

        // Adding services as Transient
        foreach (var appService in appServices)
        {
            var iAppService = iAppServices.First(i => i.IsAssignableFrom(appService));
            services.AddTransient(iAppService, appService);
        }

        return services;
    }

    private static IServiceCollection ConfigureViewsByReflection(this IServiceCollection services)
    {
        var viewsNamespace = "Maxsys.MediaManager.MusicContext.WPF.Views";
        var iViewType = typeof(IView);

        var assembly = typeof(App).Assembly;

        var views = assembly.GetTypes()
            .Where(t => t.Namespace == viewsNamespace && iViewType.IsAssignableFrom(t))
            .ToList();

        // Adding views as Transient
        views.ForEach(view => services.AddTransient(view));

        return services;
    }

    private static IServiceCollection ConfigureCommands(this IServiceCollection services)
    {
        services.AddTransient<CloseMainContentCommand>();
        services.AddTransient<OpenViewCommand>();
        services.AddTransient<CloseAppCommand>();

        return services;
    }

    private static IServiceCollection ConfigureMainViewServices(this IServiceCollection services)
    {
        services.AddSingleton<IDialogService, MainWindowToolbarDialogService>();
        services.AddSingleton<IQuestionDialogService, GlobalQuestionDialogService>();
        services.AddSingleton<IMainContentCloser, MainWindowContentCloser>();

        return services;
    }
}