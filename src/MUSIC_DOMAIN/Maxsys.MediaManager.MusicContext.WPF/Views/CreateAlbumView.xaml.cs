using Maxsys.MediaManager.CoreDomain.Interfaces;
using Maxsys.MediaManager.MusicContext.ApplicationMVVM.ViewModels;
using Maxsys.ModelCore.Interfaces.Services;
using Microsoft.Extensions.Logging;
using System.Windows.Controls;

namespace Maxsys.MediaManager.MusicContext.WPF.Views
{
    public partial class CreateAlbumView : UserControl, IView
    {
        //private readonly CreateAlbumViewModel _viewModel;

        //public CreateAlbumView(
        //    ILogger<CreateAlbumView> logger,
        //    IDialogService dialogService,
        //    IMainContentCloser contentCloser,
        //    ICreateAlbumAppService appService)
        //{
        //    InitializeComponent();

        //    DataContext = _viewModel = new(logger, dialogService, contentCloser, appService);

        //    Loaded += async (s, o) => await _viewModel.LoadedCatalogsAsync();
        //}
    }
}