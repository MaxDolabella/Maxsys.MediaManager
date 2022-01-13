using System.Threading.Tasks;

namespace Maxsys.MediaManager.MusicContext.DataExporterConsole.Commands
{
    internal interface ICommandAsync
    {
        Task ExecuteAsync();
    }
}