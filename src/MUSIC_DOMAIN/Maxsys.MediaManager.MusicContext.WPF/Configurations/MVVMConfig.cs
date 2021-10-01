using Maxsys.MediaManager.CoreDomain.Interfaces;
using Maxsys.MediaManager.MusicContext.WPF.Services;
using Maxsys.ModelCore.Interfaces.Services;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;

namespace Maxsys.MediaManager.MusicContext.WPF.Configurations
{
    public static class MVVMConfig
    {
        public static void AddMVVMConfiguration(this IServiceCollection services)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));

            // AppServices
            services.AddAppServicesByReflection();

            // Views
            services.AddViewsByReflection();
            // Main Window as Singleton
            services.AddSingleton<MainWindow>();

            services.AddSingleton<IDialogService, MainWindowToolbarDialogService>();
            services.AddSingleton<IQuestionDialogService, GlobalQuestionDialogService>();
            services.AddSingleton<IMainContentCloser, MainWindowContentCloser>();
        }

        private static void AddViewsByReflection(this IServiceCollection services)
        {
            var viewsNamespace = "Maxsys.MediaManager.MusicContext.WPF.Views";
            var iViewType = typeof(IView);

            var assembly = typeof(App).Assembly;

            var views = assembly.GetTypes()
                .Where(t => t.Namespace == viewsNamespace && iViewType.IsAssignableFrom(t))
                .ToList();

            // Adding views as Transient
            views.ForEach(view => services.AddTransient(view));
        }

        private static void AddAppServicesByReflection(this IServiceCollection services)
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
        }
    }
}