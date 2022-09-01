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
    /// <summary>
    /// Interaction logic for DeleteArtistView.xaml
    /// </summary>
    public partial class DeleteArtistView : UserControl, IView
    {
        private readonly DeleteArtistViewModel _viewModel;

        public DeleteArtistView(
            ILogger<DeleteArtistView> logger,
            IDialogService dialogService,
            IQuestionDialogService questionDialogService,
            IMainContentCloser contentCloser,
            IPathService pathService,
            IDeleteArtistAppService appService)
        {
            InitializeComponent();

            DataContext = _viewModel = new(logger, dialogService, questionDialogService, contentCloser, pathService, appService);

            Loaded += async (s, o) => await _viewModel.LoadedCatalogsAsync();
        }
    }
}
