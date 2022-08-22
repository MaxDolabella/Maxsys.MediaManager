using FluentValidation.Results;
using Maxsys.MediaManager.CoreDomain.Commands;
using Maxsys.MediaManager.MusicContext.ApplicationMVVM.Interfaces.Services;
using Maxsys.MediaManager.MusicContext.ApplicationMVVM.ViewModels;
using Maxsys.ModelCore.Interfaces.Services;
using Microsoft.Extensions.Logging;
using System.ComponentModel;
using System.Threading.Tasks;

namespace Maxsys.MediaManager.MusicContext.ApplicationMVVM.Commands
{
    public class CreateArtistCommand : CommandBase
    {
        private readonly ILogger _logger;
        private readonly CreateArtistViewModel _viewModel;
        private readonly ICreateArtistAppService _appService;
        private readonly IDialogService _dialogService;

        public CreateArtistCommand(
            CreateArtistViewModel viewModel,
            ILogger logger,
            ICreateArtistAppService appService,
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

        

        private void RedefineModel()
        {
            DenotifyPropertyChanges();
            _viewModel.Model = new();
            NotifyPropertyChanges();
            OnCanExecuteChanged();
        }

        private async Task OnArtistSaved()
        {
            var message = $"Song catalog [{_viewModel.Model.Name}] registered!";

            _logger.LogInformation(message);
            _dialogService.ShowMessage(MessageType.Information, message);

            RedefineModel();

            await _viewModel.ViewLoadedAsync();
        }

        private void OnArtistSaveFail(ValidationResult validationResult)
        {
            var message = $"Error while registering artist:\n{validationResult}";

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
            return _viewModel.Model is not null
                && base.CanExecute(parameter);
        }

        public override async void Execute(object parameter)
        {
            _logger.LogInformation($"Registering Artist [{_viewModel.Model.Name}].");

            // Validate Model
            if (!_viewModel.Model.IsValid)
            {
                OnArtistSaveFail(_viewModel.Model.ValidationResult);
                return;
            }

            // Execute saving
            var validationResult = await _appService.AddNewArtistAsync(_viewModel.Model);
            if (validationResult.IsValid)
                await OnArtistSaved();
            else
                OnArtistSaveFail(validationResult);
        }
    }
}