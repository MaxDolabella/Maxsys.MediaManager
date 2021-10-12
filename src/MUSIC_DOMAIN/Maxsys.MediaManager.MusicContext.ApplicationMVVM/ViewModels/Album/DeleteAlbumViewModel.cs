using CommunityToolkit.Mvvm.Input;
using Maxsys.MediaManager.CoreDomain.Interfaces;
using Maxsys.MediaManager.MusicContext.ApplicationMVVM.Commands;
using Maxsys.MediaManager.MusicContext.ApplicationMVVM.Interfaces.Services;
using Maxsys.MediaManager.MusicContext.ApplicationMVVM.Models;
using Maxsys.ModelCore.Interfaces.Services;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Maxsys.MediaManager.MusicContext.ApplicationMVVM.ViewModels
{
    public sealed class DeleteAlbumViewModel : ViewModelCollectionBase<AlbumListModel>
    {
        #region FIELDS

        private readonly IDeleteAlbumAppService _appService;

        private AlbumListModel _selectedModel;

        #endregion FIELDS

        #region PROPS

        public AlbumListModel SelectedModel
        {
            get => _selectedModel;
            set => SetProperty(ref _selectedModel, value);
        }

        #endregion PROPS

        #region COMMANDS

        public ICommand DeleteAlbumCommand { get; }
        public ICommand OpenFolderCommand { get; }

        #endregion COMMANDS

        #region METHODS

        public override async Task ViewLoadedAsync()
        {
            await LoadAlbumsAsync();
        }

        private async Task LoadAlbumsAsync()
        {
            _logger.LogDebug("Loading Albums.");

            Models = (await _appService.GetAlbumsAsync()).ToObservableCollection();

            _logger.LogDebug("Albums loaded.");
        }

        private void OpenFolderAction()
        {
            Process.Start("explorer.exe", SelectedModel.AlbumDirectory);
        }

        #endregion METHODS

        #region CTOR

        public DeleteAlbumViewModel(
            ILogger logger,
            IQuestionDialogService questionDialogService,
            IDialogService dialogService,
            IMainContentCloser contentCloser,
            IDeleteAlbumAppService appService)
            : base(logger, dialogService, contentCloser)
        {
            _appService = appService;

            DeleteAlbumCommand = new DeleteAlbumCommand(this, logger, questionDialogService, dialogService, appService);
            OpenFolderCommand = new RelayCommand(OpenFolderAction);
        }

        

        #endregion CTOR
    }
}