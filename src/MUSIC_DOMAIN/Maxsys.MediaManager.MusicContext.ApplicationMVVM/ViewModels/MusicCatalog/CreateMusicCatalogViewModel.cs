using CommunityToolkit.Mvvm.Input;
using Maxsys.MediaManager.CoreDomain.Interfaces;
using Maxsys.MediaManager.MusicContext.ApplicationMVVM.Interfaces.Services;
using Maxsys.MediaManager.MusicContext.ApplicationMVVM.Models;
using Maxsys.ModelCore.Interfaces.Services;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using ValidationResult = FluentValidation.Results.ValidationResult;

namespace Maxsys.MediaManager.MusicContext.ApplicationMVVM.ViewModels
{
    public class CreateMusicCatalogViewModel : ViewModelBase<CreateMusicCatalogModel>
    {
        #region FIELDS

        private readonly ICreateMusicCatalogAppService _appService;

        private ReadOnlyObservableCollection<MusicCatalogInfoModel> _musicCatalogs;

        #endregion FIELDS

        #region PROPS

        public ReadOnlyObservableCollection<MusicCatalogInfoModel> MusicCatalogs
        {
            get => _musicCatalogs;
            private set => SetProperty(ref _musicCatalogs, value);
        }

        #endregion PROPS

        #region COMMANDS

        public ICommand SaveCommand { get; }
        public ICommand CloseCommand { get; }

        #endregion COMMANDS

        #region METHODS

        public override async Task ViewLoadedAsync()
        {
            await LoadMusicCatalogsAsync();
        }

        private async Task LoadMusicCatalogsAsync()
        {
            _logger.LogInformation("Loading registered MusicCatalogs.");

            MusicCatalogs = (await _appService.GetMusicCatalogsAsync()).ToReadOnlyObservableCollection();

            _logger.LogInformation("Registered MusicCatalogs loaded.");
        }

        // TODO create AsyncRelayCommand for ValueTask?
        private async Task<ValidationResult> SaveAction()
        {
            _logger.LogInformation("Registering MusicCatalog [{Name}].", Model.Name);

            var validationResult = Model.IsValid
                ? await _appService.AddNewMusicCatalogAsync(Model)
                : await Task.FromResult(Model.ValidationResult);

            if (validationResult.IsValid)
                await OnMusicCatalogSaved();
            else
                OnMusicCatalogSaveFail(validationResult);

            return validationResult;
        }

        private async Task OnMusicCatalogSaved()
        {
            var message = $"Music catalog [{Model.Name}] registered!";

            _logger.LogInformation(message);
            _dialogService.ShowMessage(MessageType.Information, message);

            Model = new();

            await LoadMusicCatalogsAsync();
        }

        private void OnMusicCatalogSaveFail(ValidationResult validationResult)
        {
            var message = $"Error while registering Music Catalog: {validationResult}";

            _dialogService.ShowMessage(MessageType.Error, message);
            _logger.LogError(message);
        }

        private void CloseAction()
        {
            _mainContentCloser.CloseMainContent();
        }

        #endregion METHODS

        #region CTOR

        public CreateMusicCatalogViewModel(
            ILogger logger,
            IDialogService dialogService,
            IMainContentCloser contentCloser,
            ICreateMusicCatalogAppService appService)
            : base(logger, dialogService, contentCloser)
        {
            _appService = appService;

            SaveCommand = new AsyncRelayCommand(SaveAction);
            CloseCommand = new RelayCommand(CloseAction);
        }

        #endregion CTOR
    }
}