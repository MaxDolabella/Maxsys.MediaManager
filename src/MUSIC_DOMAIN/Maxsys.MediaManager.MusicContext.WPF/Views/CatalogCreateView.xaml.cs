using Maxsys.MediaManager.CoreDomain.Interfaces;
using Maxsys.MediaManager.MusicContext.ApplicationMVVM.Interfaces.ViewModels;
using System.Windows.Controls;

namespace Maxsys.MediaManager.MusicContext.WPF.Views
{
    public partial class CreateMusicCatalogView : UserControl, IView
    {
        private readonly ICatalogCreateViewModel _viewModel;

        public CreateMusicCatalogView(ICatalogCreateViewModel viewModel)
        {
            InitializeComponent();

            DataContext = _viewModel = viewModel;

            Loaded += async (s, o) => await _viewModel.LoadCatalogsAsync();
        }
    }
}