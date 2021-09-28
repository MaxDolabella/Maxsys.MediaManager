using Maxsys.Core.Helpers;
using Maxsys.MediaManager.CoreDomain.Interfaces;
using Maxsys.MediaManager.MusicContext.ApplicationMVVM.Interfaces;
using Maxsys.MediaManager.MusicContext.ApplicationMVVM.Interfaces.Services;
using Maxsys.MediaManager.MusicContext.ApplicationMVVM.ViewModels;
using Maxsys.ModelCore.Interfaces.Services;
using Microsoft.Extensions.Logging;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using Image = System.Drawing.Image;

namespace Maxsys.MediaManager.MusicContext.WPF.Views
{
    public partial class CreateAlbumView : UserControl, IView
    {
        private readonly CreateAlbumViewModel _viewModel;
        private readonly ILogger _logger;

        public CreateAlbumView(
            ILogger<CreateAlbumView> logger,
            IDialogService dialogService,
            IMainContentCloser contentCloser,
            ICreateAlbumAppService appService)
        {
            InitializeComponent();

            _logger = logger;

            DataContext = _viewModel = new(logger, dialogService, contentCloser, appService);

            Loaded += async (s, o) => await _viewModel.ViewLoadedAsync();
        }

        #region Drag and Drop

        private void OnPicBoxAlbumCover_DragDrop(object sender, DragEventArgs e)
        {
            _logger.LogInformation("Entering OnPicBoxAlbumCover_DragDrop.");
            // still check if the associated data from the file(s) can be used for this purpose
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                var filePath = ((string[])e.Data.GetData(DataFormats.FileDrop))[0];
                if (Path.GetExtension(filePath).ToLower() == ".jpg")
                {
                    _viewModel.Model.AlbumCover = Image.FromFile(filePath).ImageToBytes();
                }
            }
            _logger.LogInformation("Leaving OnPicBoxAlbumCover_DragDrop.");
        }

        private void OnPicBoxAlbumCover_DragEnter(object sender, DragEventArgs e)
        {
            _logger.LogInformation("Entering OnPicBoxAlbumCover_DragEnter.");
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
            _logger.LogInformation("Leaving OnPicBoxAlbumCover_DragEnter.");
        }

        #endregion Drag and Drop
    }
}