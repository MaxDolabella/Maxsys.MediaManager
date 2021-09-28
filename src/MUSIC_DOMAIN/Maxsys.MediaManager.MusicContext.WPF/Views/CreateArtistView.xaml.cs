using Maxsys.MediaManager.CoreDomain.Interfaces;
using Maxsys.MediaManager.MusicContext.ApplicationMVVM.Interfaces;
using Maxsys.MediaManager.MusicContext.ApplicationMVVM.Interfaces.Services;
using Maxsys.MediaManager.MusicContext.ApplicationMVVM.ViewModels;
using Maxsys.ModelCore.Interfaces.Services;
using Microsoft.Extensions.Logging;
using System.Windows.Controls;

namespace Maxsys.MediaManager.MusicContext.WPF.Views
{
    public partial class CreateArtistView : UserControl, IView
    {
        private readonly CreateArtistViewModel _viewModel;

        public CreateArtistView(
            ILogger<CreateArtistView> logger,
            IDialogService dialogService,
            IMainContentCloser contentCloser,
            ICreateArtistAppService appService)
        {
            InitializeComponent();

            DataContext = _viewModel = new(logger, dialogService, contentCloser, appService);

            Loaded += async (s, o) => await _viewModel.ViewLoadedAsync();
        }
    }
}