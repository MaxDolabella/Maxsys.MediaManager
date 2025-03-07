using Microsoft.Extensions.Configuration;

namespace Maxsys.Core.Data.Extensions;

// TODO Mover pra Core
public static class IConfigurationExtensions
{
    /// <summary>
    /// Gets the connection string with the same name of DbContext.
    /// Shorthand for <c>GetSection("ConnectionStrings")[typeof(TContext).Name]</c>.
    /// </summary>
    /// <param name="configuration">The configuration to enumerate.</param>
    /// <typeparam name="TContext">Is the Context</typeparam>
    /// <returns>The connection string.</returns>
    public static string? GetConnectionString<TContext>(this IConfiguration configuration)
        where TContext : DbContext
    {
        return configuration.GetConnectionString(typeof(TContext).Name);
    }
}