using Maxsys.MediaManager.CoreDomain.Commands;
using Maxsys.MediaManager.CoreDomain.Interfaces;

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