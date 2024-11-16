using System;
using Microsoft.Extensions.Configuration;

namespace Maxsys.MediaManager.MusicContext.DataExporterConsole.Configurations;

internal static class DomainPropertiesConfigurations
{
    internal static void SetDomainProperties(IConfiguration configuration)
    {
        AppDomain.CurrentDomain.SetData("DataDirectory", configuration["CoreSettings:DataDirectory"]);
    }
}