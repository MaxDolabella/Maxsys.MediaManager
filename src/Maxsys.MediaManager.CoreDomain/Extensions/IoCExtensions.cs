using Maxsys.MediaManager.CoreDomain.Interfaces.Services;
using Maxsys.MediaManager.CoreDomain.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Maxsys.MediaManager.CoreDomain.Extensions;

public static class IoCExtensions
{
    /// <summary>
    /// Adds dependecy injection from Maxsys.MediaManager.CoreDomain
    /// </summary>
    public static IServiceCollection AddCoreDependencyInjection(this IServiceCollection services, ServiceLifetime lifetime = ServiceLifetime.Scoped)
    {
        //services.Add(new ServiceDescriptor(typeof(IFilePropertiesReader), typeof(FilePropertiesReader), lifetime));
        services.Add<IFilePropertiesReader, FilePropertiesReader>(lifetime);

        return services;
    }
}