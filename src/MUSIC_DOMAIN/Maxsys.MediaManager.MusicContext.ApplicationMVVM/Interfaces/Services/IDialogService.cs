using Maxsys.Core.Interfaces.Services;

namespace Maxsys.MediaManager.MusicContext.WPF.Services;

public interface IDialogService : IService
{
    object Owner { get; set; }

    void ShowMessage(MessageType messageType, string message, string title = "");
}
