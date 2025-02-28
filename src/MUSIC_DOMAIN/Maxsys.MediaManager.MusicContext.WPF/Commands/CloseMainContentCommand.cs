using Maxsys.MediaManager.CoreDomain.Interfaces;
using Maxsys.MediaManager.MusicContext.WPF.Commands.Abstrations;

namespace Maxsys.MediaManager.MusicContext.WPF.Commands;

public class CloseMainContentCommand : CommandBase
{
    protected readonly IMainContentCloser _mainContentCloser;

    public CloseMainContentCommand(IMainContentCloser mainContentCloser)
    {
        _mainContentCloser = mainContentCloser;
    }

    public override void Execute(object? parameter)
    {
        _mainContentCloser?.CloseMainContent();
    }
}