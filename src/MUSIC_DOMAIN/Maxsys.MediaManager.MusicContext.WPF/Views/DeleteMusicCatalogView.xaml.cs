using Maxsys.MediaManager.CoreDomain.Interfaces;
using Maxsys.MediaManager.MusicContext.ApplicationMVVM.Interfaces;
using Maxsys.MediaManager.MusicContext.ApplicationMVVM.Interfaces.Services;
using Maxsys.MediaManager.MusicContext.ApplicationMVVM.ViewModels;
using Maxsys.MediaManager.MusicContext.Domain.Interfaces.Services;
using Maxsys.ModelCore.Interfaces.Services;
using Microsoft.Extensions.Logging;
using System.Windows.Controls;

namespace Maxsys.MediaManager.MusicContext.WPF.Views
{
    public partial class DeleteMusicCatalogView : UserControl, IView
    {
        private readonly DeleteMusicCatalogViewModel _viewModel;

        public DeleteMusicCatalogView(
            ILogger<DeleteMusicCatalogView> logger,
            IDialogService dialogService,
            IMainContentCloser contentCloser,
            IPathService pathService,
            IDeleteMusicCatalogAppService appService)
        {
            InitializeComponent();

            DataContext = _viewModel = new(logger, dialogService, contentCloser, pathService, appService);

            Loaded += async (s, o) => await _viewModel.ViewLoadedAsync();
        }
    }
}