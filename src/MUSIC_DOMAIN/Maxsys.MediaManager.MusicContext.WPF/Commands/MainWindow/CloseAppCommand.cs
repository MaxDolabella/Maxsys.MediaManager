using Maxsys.MediaManager.CoreDomain.Interfaces;
using Maxsys.MediaManager.MusicContext.WPF.Commands.Abstrations;
using Microsoft.Extensions.Logging;

namespace Maxsys.MediaManager.MusicContext.WPF.Commands.MainWindow;

public class CloseAppCommand : CommandBase
{
    private readonly ILogger _logger;
    private readonly IAppCloser _appCloser;

    public CloseAppCommand(ILogger logger, IAppCloser appCloser)
    {
        _logger = logger;
        _appCloser = appCloser;
    }

    public override void Execute(object parameter)
    {
        _logger.LogInformation("Closing the application");

        _appCloser.CloseApp();
    }
}