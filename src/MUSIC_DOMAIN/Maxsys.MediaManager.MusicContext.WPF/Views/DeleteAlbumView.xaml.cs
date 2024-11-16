using Maxsys.MediaManager.CoreDomain.Interfaces;
using Maxsys.MediaManager.MusicContext.ApplicationMVVM.ViewModels;
using Maxsys.ModelCore.Interfaces.Services;
using Microsoft.Extensions.Logging;
using System.Windows.Controls;

namespace Maxsys.MediaManager.MusicContext.WPF.Views
{
    public partial class DeleteAlbumView : UserControl, IView
    {
        //private readonly DeleteAlbumViewModel _viewModel;

        //public DeleteAlbumView(
        //    ILogger<DeleteAlbumView> logger,
        //    IQuestionDialogService questionDialogService,
        //    IDialogService dialogService,
        //    IMainContentCloser contentCloser,
        //    IDeleteAlbumAppService appService)
        //{
        //    InitializeComponent();

        //    DataContext = _viewModel = new(logger, questionDialogService, dialogService, contentCloser, appService);

        //    Loaded += async (s, o) => await _viewModel.LoadedCatalogsAsync();
        //}
    }
}