using CommunityToolkit.Mvvm.Input;
using FluentValidation.Results;
using Maxsys.MediaManager.CoreDomain.Interfaces;
using Maxsys.MediaManager.MusicContext.ApplicationMVVM.Interfaces.Services;
using Maxsys.MediaManager.MusicContext.ApplicationMVVM.Models;
using Maxsys.ModelCore.Interfaces.Services;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;
using ValidationResult = FluentValidation.Results.ValidationResult;

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
        public ICommand CloseCommand { get; }

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

        private async Task<ValidationResult> DeleteAlbumAction()
        {
            var validationResult = ValidateSelectedModel();
            if (!validationResult.IsValid)
                return await Task.FromResult(validationResult);

            _logger.LogInformation($"Deleting album [{SelectedModel.AlbumName}]...");

            validationResult = await _appService.DeleteAlbumAsync(SelectedModel);

            if (validationResult.IsValid)
                await OnAlbumDeleted();
            else
                OnAlbumDeleteFail(validationResult);

            return validationResult;
        }

        private ValidationResult ValidateSelectedModel()
        {
            var validationResult = new ValidationResult();
            
            if (SelectedModel is null)
            {
                var message = "No item is selected.";
                
                validationResult.AddFailure(message);
                _dialogService.ShowMessage(MessageType.Warning, message);

                return validationResult;
            }

            if (!SelectedModel.IsValid)
            {
                var message = $"Selected item is invalid:\n{SelectedModel.ValidationResult}";
                
                _dialogService.ShowMessage(MessageType.Warning, message);
                
                return SelectedModel.ValidationResult;
            }

            return validationResult;
        }

        private async Task OnAlbumDeleted()
        {
            var message = $"Album [{SelectedModel.AlbumName}] Deleted!";

            _logger.LogInformation(message);
            _dialogService.ShowMessage(MessageType.Information, message);

            _ = DeleteAlbumDirectory().ConfigureAwait(false);

            await ViewLoadedAsync();
        }

        private void OnAlbumDeleteFail(ValidationResult validationResult)
        {
            var message = $"Error while deleting album:\n{validationResult}";

            _logger.LogError(message);
            _dialogService.ShowMessage(MessageType.Error, message);
        }

        private async Task DeleteAlbumDirectory()
        {
            await Task.Run(() =>
            {
                try
                {
                    _logger.LogDebug($"Deleting folder <{SelectedModel.AlbumDirectory}>.");

                    System.IO.Directory.Delete(SelectedModel.AlbumDirectory, true);

                    _logger.LogWarning($"Folder deleted.");
                }
                catch (System.Exception ex)
                {
                    _logger.LogError("Folder cannot be deleted: {errors}", ex.Message);
                }
            });
        }

        private void CloseAction()
        {
            _mainContentCloser.CloseMainContent();
        }

        #endregion METHODS

        #region CTOR

        public DeleteAlbumViewModel(
            ILogger logger,
            IDialogService dialogService,
            IMainContentCloser contentCloser,
            IDeleteAlbumAppService appService)
            : base(logger, dialogService, contentCloser)
        {
            _appService = appService;

            DeleteAlbumCommand = new AsyncRelayCommand(DeleteAlbumAction);
            CloseCommand = new RelayCommand(CloseAction);
        }

        #endregion CTOR
    }
}