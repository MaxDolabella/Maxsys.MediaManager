using Maxsys.MediaManager.CoreDomain.Interfaces;
using Maxsys.MediaManager.MusicContext.ApplicationMVVM.Interfaces;
using Maxsys.MediaManager.MusicContext.ApplicationMVVM.Interfaces.Services;
using Maxsys.MediaManager.MusicContext.ApplicationMVVM.ViewModels;
using Maxsys.ModelCore.Interfaces.Services;
using Microsoft.Extensions.Logging;
using System.Windows.Controls;

namespace Maxsys.MediaManager.MusicContext.WPF.Views
{
    public partial class CreateMusicCatalogView : UserControl, IView
    {
        private readonly CreateMusicCatalogViewModel _viewModel;

        public CreateMusicCatalogView(
            ILogger<CreateMusicCatalogView> logger,
            IDialogService dialogService,
            IMainContentCloser contentCloser,
            ICreateMusicCatalogAppService appService)
        {
            InitializeComponent();

            DataContext = _viewModel = new(logger, dialogService, contentCloser, appService);

            Loaded += async (s, o) => await _viewModel.ViewLoadedAsync();
        }
    }
}