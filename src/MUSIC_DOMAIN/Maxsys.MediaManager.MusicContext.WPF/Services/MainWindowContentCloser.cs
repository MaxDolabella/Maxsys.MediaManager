using Maxsys.MediaManager.CoreDomain.Interfaces;
using System;

namespace Maxsys.MediaManager.MusicContext.WPF.Services
{
    public class MainWindowContentCloser : IMainContentCloser
    {
        private readonly MainWindow _window;

        public MainWindowContentCloser(MainWindow window)
        {
            _window = window;
        }

        public void CloseMainContent()
        {
            if (_window.ContentContainer.Content is not null)
            {
                var currentView = _window.ContentContainer.Content;
                _window.ContentContainer.Content = null;
                GC.SuppressFinalize(currentView);
            }
        }
    }
}