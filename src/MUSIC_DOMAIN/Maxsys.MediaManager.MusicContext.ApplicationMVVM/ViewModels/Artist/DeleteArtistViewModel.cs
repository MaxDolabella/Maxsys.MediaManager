using CommunityToolkit.Mvvm.Input;
using FluentValidation.Results;
using Maxsys.MediaManager.CoreDomain.Interfaces;
using Maxsys.MediaManager.MusicContext.ApplicationMVVM.Interfaces.Services;
using Maxsys.MediaManager.MusicContext.ApplicationMVVM.Models;
using Maxsys.MediaManager.MusicContext.Domain.DTO;
using Maxsys.MediaManager.MusicContext.Domain.Interfaces.Services;
using Maxsys.ModelCore.Interfaces.Services;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;
using ValidationResult = FluentValidation.Results.ValidationResult;

namespace Maxsys.MediaManager.MusicContext.ApplicationMVVM.ViewModels
{
    public sealed class DeleteArtistViewModel : ViewModelCollectionBase<ArtistListModel>
    {
        #region FIELDS

        private readonly IDeleteArtistAppService _appService;
        private readonly IPathService _pathService;

        private ArtistListModel _selectedModel;

        #endregion FIELDS

        #region PROPS

        public ArtistListModel SelectedModel
        {
            get => _selectedModel;
            set => SetProperty(ref _selectedModel, value);
        }

        #endregion PROPS

        #region COMMANDS

        public ICommand DeleteArtistCommand { get; }
        public ICommand CloseCommand { get; }

        #endregion COMMANDS

        #region METHODS

        public override async Task ViewLoadedAsync()
        {
            await LoadArtistsAsync();
        }

        private async Task LoadArtistsAsync()
        {
            _logger.LogDebug("Loading Artists.");

            Models = (await _appService.GetArtistsAsync()).ToObservableCollection();

            _logger.LogDebug("Artists loaded.");
        }

        private async Task<ValidationResult> DeleteArtistAction()
        {
            var validationResult = ValidateSelectedModel();
            if (!validationResult.IsValid)
                return await Task.FromResult(validationResult);

            _logger.LogInformation($"Deleting artist [{SelectedModel.ArtistName}]...");

            validationResult = await _appService.DeleteArtistAsync(SelectedModel);

            if (validationResult.IsValid)
                await OnArtistDeleted();
            else
                OnArtistDeleteFail(validationResult);

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

        private async Task OnArtistDeleted()
        {
            var message = $"Artist [{SelectedModel.ArtistName}] Deleted!";

            _logger.LogInformation(message);
            _dialogService.ShowMessage(MessageType.Information, message);

            _ = DeleteArtistDirectory().ConfigureAwait(false);

            await ViewLoadedAsync();
        }

        private void OnArtistDeleteFail(ValidationResult validationResult)
        {
            var message = $"Error while deleting artist:\n{validationResult}";

            _logger.LogError(message);
            _dialogService.ShowMessage(MessageType.Error, message);
        }

        private async Task DeleteArtistDirectory()
        {
            await Task.Run(() =>
            {
                try
                {
                    var dto = new DefineArtistFolderDTO
                    {
                        ArtistName = SelectedModel.ArtistName,
                        MusicCatalogName = SelectedModel.MusicCatalogName
                    };

                    var artistDirectory = _pathService.GetArtistDirectory(dto);

                    _logger.LogDebug($"Deleting folder <{artistDirectory}>.");

                    System.IO.Directory.Delete(artistDirectory, true);

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

        public DeleteArtistViewModel(
            ILogger logger,
            IDialogService dialogService,
            IMainContentCloser contentCloser,
            IPathService pathService,
            IDeleteArtistAppService appService)
            : base(logger, dialogService, contentCloser)
        {
            _appService = appService;

            DeleteArtistCommand = new AsyncRelayCommand(DeleteArtistAction);
            CloseCommand = new RelayCommand(CloseAction);
            _pathService = pathService;
        }

        #endregion CTOR
    }
}