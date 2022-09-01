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
            IQuestionDialogService questionDialogService,
            IMainContentCloser contentCloser,
            IDeleteMusicCatalogAppService appService)
        {
            InitializeComponent();

            DataContext = _viewModel = new(logger, dialogService, questionDialogService, contentCloser, appService);

            Loaded += async (s, o) => await _viewModel.LoadedCatalogsAsync();
        }
    }
}