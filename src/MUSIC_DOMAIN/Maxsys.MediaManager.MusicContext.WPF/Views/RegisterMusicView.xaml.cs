using Maxsys.MediaManager.CoreDomain.Interfaces;
using Maxsys.MediaManager.MusicContext.ApplicationMVVM.Interfaces;
using Maxsys.MediaManager.MusicContext.ApplicationMVVM.Interfaces.Services;
using Maxsys.MediaManager.MusicContext.ApplicationMVVM.Models;
using Maxsys.MediaManager.MusicContext.ApplicationMVVM.ViewModels;
using Maxsys.ModelCore.Interfaces.Services;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Maxsys.MediaManager.MusicContext.WPF.Views
{
    public partial class RegisterMusicView : UserControl, IView
    {
        private readonly RegisterMusicViewModel _viewModel;

        public RegisterMusicView(
            ILogger<RegisterMusicView> logger,
            IDialogService dialogService,
            IMainContentCloser contentCloser,
            IRegisterMusicAppService appService)
        {
            InitializeComponent();

            DataContext = _viewModel = new(logger, dialogService, contentCloser, appService);

            dgvMusics.SelectionChanged += DgvMusics_SelectionChanged;
            Loaded += async (s, o) => await _viewModel.ViewLoadedAsync();
        }

        private void DgvMusics_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _viewModel.SelectedModels
                = dgvMusics.SelectedItems.Cast<CreateMusicModel>().ToReadOnlyObservableCollection();
        }

        #region Drag and Drop

        private void OnDgvMusics_DragEnter(object sender, DragEventArgs e)
        {
            // Check if the Data format of the file(s) can be accepted
            // (we only accept file drops from Windows Explorer, etc.)
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                // modify the drag drop effects to Move
                e.Effects = DragDropEffects.Move;
            }
            else
            {
                // no need for any drag drop effect
                e.Effects = DragDropEffects.None;
            }
        }

        private void OnDgvMusics_DragDrop(object sender, DragEventArgs e)
        {
            // still check if the associated data from the file(s) can be used for this purpose
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                var dropDownObj = (string[])e.Data.GetData(DataFormats.FileDrop);
                _viewModel.AddMp3FilesAction(dropDownObj);
            }
        }

        #endregion Drag and Drop
    }
}