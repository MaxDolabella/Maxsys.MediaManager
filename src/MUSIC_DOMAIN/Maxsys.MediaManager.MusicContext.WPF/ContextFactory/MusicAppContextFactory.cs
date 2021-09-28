using Maxsys.MediaManager.MusicContext.Infra.DataEFCore.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;

namespace Maxsys.MediaManager.MusicContext.WPF
{
    /// <summary>
    /// Used to Design-time DbContext Creation (Migrations)<br/>
    /// <see href="https://stackoverflow.com/a/68383155/4121969">stackoverflow.com</see>
    /// </summary>
    public class MusicAppContextFactory : IDesignTimeDbContextFactory<MusicAppContext>
    {
        public MusicAppContext CreateDbContext(string[] args)
        {
            // WARNING appsettings.json
            var jsonFile = "appsettings.Development.json";
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile(jsonFile, optional: false, reloadOnChange: true);

            var configuration = builder.Build();

            AppDomain.CurrentDomain.SetData("DataDirectory", configuration["CoreSettings:DataDirectory"]);

            var conn = configuration.GetConnectionString(nameof(MusicAppContext));

            var optionsBuilder = new DbContextOptionsBuilder<MusicAppContext>();
            optionsBuilder.UseSqlServer(conn, options => options.EnableRetryOnFailure());

            return new MusicAppContext(optionsBuilder.Options);
        }
    }
}