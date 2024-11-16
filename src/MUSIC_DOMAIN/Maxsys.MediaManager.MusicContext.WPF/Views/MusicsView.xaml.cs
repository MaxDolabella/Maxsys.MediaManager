using Maxsys.MediaManager.CoreDomain.Interfaces;
using Maxsys.MediaManager.MusicContext.ApplicationMVVM.ViewModels;
using Maxsys.ModelCore.Interfaces.Services;
using Microsoft.Extensions.Logging;
using System.Windows.Controls;

namespace Maxsys.MediaManager.MusicContext.WPF.Views
{
    public partial class MusicsView : UserControl, IView
    {
        //private readonly MusicsViewModel _viewModel;

        //public MusicsView(
        //    ILogger<MusicsView> logger,
        //    IDialogService dialogService,
        //    IQuestionDialogService questionDialogService,
        //    IMainContentCloser contentCloser,
        //    IMusicListAppService appService)
        //{
        //    InitializeComponent();

        //    DataContext = _viewModel = new(logger, dialogService, questionDialogService, contentCloser, appService);

        //    Loaded += async (s, o) => await _viewModel.LoadedCatalogsAsync();
        //    FixDataGridMusicsColumnsSize();
        //}

        //private void FixDataGridMusicsColumnsSize()
        //{

        //    for (int i = 0; i < DataGridMusics.Columns.Count; i++)
        //    {
        //        var col = DataGridMusics.Columns[i];

        //        col.Width = i == 5
        //            ? new DataGridLength(1, DataGridLengthUnitType.Star)
        //            : DataGridLength.Auto;
        //    }


        //    DataGridMusics.UpdateLayout();
        //}
    }
}