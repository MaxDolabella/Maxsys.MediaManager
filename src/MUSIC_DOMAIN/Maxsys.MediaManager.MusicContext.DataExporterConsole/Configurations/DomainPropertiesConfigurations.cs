using Microsoft.Extensions.Configuration;
using System;

namespace Maxsys.MediaManager.MusicContext.DataExporterConsole.Configurations
{
    internal static class DomainPropertiesConfigurations
    {
        internal static void SetDomainProperties(IConfiguration configuration)
        {
            AppDomain.CurrentDomain.SetData("DataDirectory", configuration["CoreSettings:DataDirectory"]);
        }
    }
}