using Maxsys.Core.Data.Extensions;
using Maxsys.MediaManager.DatabaseInitializer;
using Maxsys.MediaManager.MusicContext.Infra.DataEFCore.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var host = Host.CreateDefaultBuilder()
    .ConfigureServices((context, services) =>
    {
        var connString = context.Configuration.GetConnectionString<MusicAppContext>();
        services.AddDbContext<MusicAppContext>(options => options
            .UseSqlServer(connString)
            .UseAsyncSeeding(Seed.AsyncSeeding)
            .EnableSensitiveDataLogging(true));
    })
    .Build();

await using (var scope = host.Services.CreateAsyncScope())
{
    var context = scope.ServiceProvider.GetRequiredService<MusicAppContext>();

    // Create Script
    Console.ForegroundColor = ConsoleColor.Cyan;
    Console.WriteLine(context.Database.GenerateCreateScript());

    Console.ForegroundColor = ConsoleColor.DarkGreen;
    await context.Database.EnsureDeletedAsync();
    await context.Database.EnsureCreatedAsync();

    Console.ResetColor();
}