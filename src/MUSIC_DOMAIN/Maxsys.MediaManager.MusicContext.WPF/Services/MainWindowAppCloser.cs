using Maxsys.MediaManager.CoreDomain.Interfaces;

namespace Maxsys.MediaManager.MusicContext.WPF.Services
{
    public class MainWindowAppCloser : IAppCloser
    {
        private readonly MainWindow _window;

        public MainWindowAppCloser(MainWindow window)
        {
            _window = window;
        }

        public void CloseApp()
        {
            _window.Close();
        }
    }
}