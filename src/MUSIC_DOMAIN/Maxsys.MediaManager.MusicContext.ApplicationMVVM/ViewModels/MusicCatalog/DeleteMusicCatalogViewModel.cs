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
    public sealed class DeleteMusicCatalogViewModel : ViewModelCollectionBase<MusicCatalogListModel>
    {
        #region FIELDS

        private readonly IDeleteMusicCatalogAppService _appService;

        private MusicCatalogListModel _selectedModel;

        #endregion FIELDS

        #region PROPS

        public MusicCatalogListModel SelectedModel
        {
            get => _selectedModel;
            set => SetProperty(ref _selectedModel, value);
        }

        #endregion PROPS

        #region COMMANDS

        public ICommand DeleteMusicCatalogCommand { get; }

        #endregion COMMANDS

        #region METHODS

        public override async Task ViewLoadedAsync()
        {
            await LoadMusicCatalogsAsync();
        }

        private async Task LoadMusicCatalogsAsync()
        {
            _logger.LogDebug("Loading Music Catalogs.");

            Models = (await _appService.GetMusicCatalogsAsync()).ToObservableCollection();

            _logger.LogDebug("Music Catalogs loaded.");
        }

        private async Task<ValidationResult> DeleteMusicCatalogAction()
        {
            var validationResult = ValidateSelectedModel();
            if (!validationResult.IsValid)
                return await Task.FromResult(validationResult);

            _logger.LogInformation($"Deleting music catalog [{SelectedModel.MusicCatalogName}]...");

            validationResult = await _appService.DeleteMusicCatalogAsync(SelectedModel);

            if (validationResult.IsValid)
                await OnMusicCatalogDeleted();
            else
                OnMusicCatalogDeleteFail(validationResult);

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

        private async Task OnMusicCatalogDeleted()
        {
            var message = $"Music Catalog [{SelectedModel.MusicCatalogName}] Deleted!";

            _logger.LogInformation(message);
            _dialogService.ShowMessage(MessageType.Information, message);

            _ = _appService.DeleteMusicCatalogDirectory(SelectedModel)
                .ConfigureAwait(false);

            await ViewLoadedAsync();
        }

        private void OnMusicCatalogDeleteFail(ValidationResult validationResult)
        {
            var message = $"Error while deleting music catalog:\n{validationResult}";

            _logger.LogError(message);
            _dialogService.ShowMessage(MessageType.Error, message);
        }

        #endregion METHODS

        #region CTOR

        public DeleteMusicCatalogViewModel(
            ILogger logger,
            IDialogService dialogService,
            IMainContentCloser contentCloser,
            IDeleteMusicCatalogAppService appService)
            : base(logger, dialogService, contentCloser)
        {
            _appService = appService;

            DeleteMusicCatalogCommand = new AsyncRelayCommand(DeleteMusicCatalogAction);
        }

        #endregion CTOR
    }
}