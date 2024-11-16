using Maxsys.MediaManager.CoreDomain.Interfaces;

namespace Maxsys.MediaManager.MusicContext.WPF.Services
{
    public class MainWindowContentOwner : MainWindowContentCloser, IMainContentOwner
    {
        private readonly MainWindow _window;

        public MainWindowContentOwner(MainWindow window)
            : base(window)
        {
            _window = window;
        }

        public void SetMainContent(object content)
        {
            if (content is IView view)
                _window.ContentContainer.Content = view;
        }
    }
}