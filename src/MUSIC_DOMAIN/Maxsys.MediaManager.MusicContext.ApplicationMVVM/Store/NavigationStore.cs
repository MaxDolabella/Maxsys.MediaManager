using Maxsys.MediaManager.CoreDomain.Interfaces;

namespace Maxsys.MediaManager.MusicContext.ApplicationMVVM.Store
{
    public class NavigationStore
    {
        private IView _currentView;

        public IView CurrentView
        {
            get => _currentView;
            set => _currentView = value;
        }
    }
}