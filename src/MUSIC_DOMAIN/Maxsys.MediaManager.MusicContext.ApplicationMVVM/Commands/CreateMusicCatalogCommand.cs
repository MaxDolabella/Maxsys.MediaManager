using FluentValidation.Results;
using Maxsys.MediaManager.MusicContext.ApplicationMVVM.Interfaces.Services;
using Maxsys.MediaManager.MusicContext.ApplicationMVVM.ViewModels;
using Maxsys.ModelCore.Interfaces.Services;
using Microsoft.Extensions.Logging;
using System.ComponentModel;
using System.Threading.Tasks;

namespace Maxsys.MediaManager.MusicContext.ApplicationMVVM.Commands
{
    public class CreateMusicCatalogCommand : CommandBase
    {
        private readonly ILogger _logger;
        private readonly CreateMusicCatalogViewModel _viewModel;
        private readonly ICreateMusicCatalogAppService _appService;
        private readonly IDialogService _dialogService;

        public CreateMusicCatalogCommand(
            ILogger logger,
            CreateMusicCatalogViewModel viewModel,
            ICreateMusicCatalogAppService appService,
            IDialogService dialogService)
        {
            _logger = logger;
            _viewModel = viewModel;
            _appService = appService;
            _dialogService = dialogService;

            NotifyPropertyChanges();
        }

        private void NotifyPropertyChanges()
        {
            _viewModel.Model.PropertyChanged += OnPropertyChanged;
        }

        private void DenotifyPropertyChanges()
        {
            _viewModel.Model.PropertyChanged -= OnPropertyChanged;
        }

        private async Task OnMusicCatalogSaved()
        {
            var message = $"Music catalog [{_viewModel.Model.Name}] registered!";

            _logger.LogInformation(message);
            _dialogService.ShowMessage(MessageType.Information, message);

            RedefineModel();

            await _viewModel.ViewLoadedAsync();
        }

        private void RedefineModel()
        {
            DenotifyPropertyChanges();
            _viewModel.Model = new();
            NotifyPropertyChanges();
            OnCanExecuteChanged();
        }

        private void OnMusicCatalogSaveFail(ValidationResult validationResult)
        {
            var message = $"Error while registering Music Catalog: {validationResult}";

            _dialogService.ShowMessage(MessageType.Error, message);
            _logger.LogError(message);
        }

        private void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(_viewModel.Model.Name))
                OnCanExecuteChanged();
        }

        public override bool CanExecute(object parameter)
        {
            return !string.IsNullOrWhiteSpace(_viewModel.Model.Name)
                && base.CanExecute(parameter);
        }

        public override async void Execute(object parameter)
        {
            _logger.LogInformation($"Registering MusicCatalog [{_viewModel.Model.Name}].");

            // Validate Model
            if (!_viewModel.Model.IsValid)
            {
                OnMusicCatalogSaveFail(_viewModel.Model.ValidationResult);
                return;
            }

            // Execute saving
            var validationResult = await _appService.AddNewMusicCatalogAsync(_viewModel.Model);
            if (validationResult.IsValid)
                await OnMusicCatalogSaved();
            else
                OnMusicCatalogSaveFail(validationResult);
        }
    }
}