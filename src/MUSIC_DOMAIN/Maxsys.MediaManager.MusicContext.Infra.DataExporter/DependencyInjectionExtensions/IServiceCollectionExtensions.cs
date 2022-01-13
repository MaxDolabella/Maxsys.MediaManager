using Microsoft.Extensions.DependencyInjection;

namespace Maxsys.MediaManager.MusicContext.Infra.DataExporter.DependencyInjection
{
    public static class IServiceCollectionExtensions
    {
        public static IServiceCollection ConfigureDataExporters(this IServiceCollection services)
        {
            services.AddScoped<SQLDataExporter>();
            services.AddScoped<ExcelDataExporter>();
            services.AddScoped<TxtDataExporter>();

            return services;
        }
    }
}