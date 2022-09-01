using CommunityToolkit.Mvvm.Input;
using Maxsys.MediaManager.CoreDomain.Interfaces;
using Maxsys.MediaManager.MusicContext.ApplicationMVVM.Commands;
using Maxsys.MediaManager.MusicContext.ApplicationMVVM.Interfaces.Services;
using Maxsys.MediaManager.MusicContext.ApplicationMVVM.Models;
using Maxsys.ModelCore.Interfaces.Services;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Maxsys.MediaManager.MusicContext.ApplicationMVVM.ViewModels
{
    public sealed class DeleteMusicCatalogViewModel : ViewModelCollectionBase<MusicCatalogListModel>
    {
        #region FIELDS

        private readonly IDeleteMusicCatalogAppService _appService;

        #endregion FIELDS

        #region PROPS

        private MusicCatalogListModel _selectedModel;

        public MusicCatalogListModel SelectedModel
        {
            get => _selectedModel;
            set => SetProperty(ref _selectedModel, value);
        }

        #endregion PROPS

        #region COMMANDS

        public ICommand DeleteMusicCatalogCommand { get; }
        public ICommand OpenFolderCommand { get; }

        #endregion COMMANDS

        #region METHODS

        public override async Task LoadedCatalogsAsync()
        {
            await LoadMusicCatalogsAsync();
        }

        private async Task LoadMusicCatalogsAsync()
        {
            _logger.LogDebug("Loading Song Catalogs.");

            Models = (await _appService.GetMusicCatalogsAsync()).ToObservableCollection();

            _logger.LogDebug("Song Catalogs loaded.");
        }

        #endregion METHODS

        #region CTOR

        public DeleteMusicCatalogViewModel(
            ILogger logger,
            IDialogService dialogService,
            IQuestionDialogService questionDialogService,
            IMainContentCloser contentCloser,
            IDeleteMusicCatalogAppService appService)
            : base(logger, dialogService, contentCloser)
        {
            _appService = appService;

            DeleteMusicCatalogCommand = new DeleteMusicCatalogCommand(this, logger, questionDialogService, dialogService, appService);
            OpenFolderCommand = new RelayCommand(OpenFolderAction);
        }

        private void OpenFolderAction()
        {
            _logger.LogDebug(nameof(OpenFolderCommand));
        }

        #endregion CTOR
    }
}