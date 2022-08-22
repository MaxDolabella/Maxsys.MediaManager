using FluentValidation.Results;
using Maxsys.MediaManager.CoreDomain.Commands;
using Maxsys.MediaManager.MusicContext.ApplicationMVVM.Interfaces.Services;
using Maxsys.MediaManager.MusicContext.ApplicationMVVM.ViewModels;
using Maxsys.ModelCore.Interfaces.Services;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace Maxsys.MediaManager.MusicContext.ApplicationMVVM.Commands;

public class CreateAlbumCommand : CommandBase
{
    private readonly CreateAlbumViewModel _viewModel;

    private readonly ILogger _logger;
    private readonly ICreateAlbumAppService _appService;
    private readonly IDialogService _dialogService;

    public CreateAlbumCommand(
        CreateAlbumViewModel viewModel,
        ILogger logger,
        ICreateAlbumAppService appService,
        IDialogService dialogService)
    {
        _logger = logger;
        _viewModel = viewModel;
        _appService = appService;
        _dialogService = dialogService;
    }

    private void RedefineModel()
    {
        _viewModel.Model = new();

        OnCanExecuteChanged();
    }

    private async Task OnAlbumSaved()
    {
        var message = $"Album [{_viewModel.Model.Name}] registered.";

        _logger.LogInformation(message);
        _dialogService.ShowMessage(MessageType.Information, message);

        RedefineModel();

        await _viewModel.ViewLoadedAsync();
    }

    private void OnAlbumSavingFail(ValidationResult validationResult)
    {
        var message = $"Error while registering album:\n{validationResult}";

        _dialogService.ShowMessage(MessageType.Error, message);
        _logger.LogError(message);
    }

    public override bool CanExecute(object parameter)
    {
        return _viewModel.Model is not null
            && base.CanExecute(parameter);
    }

    public override async void Execute(object parameter)
    {
        _logger.LogInformation($"Registering album [{_viewModel.Model.Name}].");

        _viewModel.Model.SetArtist(_viewModel.SelectedArtist);

        // Validate Model
        if (!_viewModel.Model.IsValid)
        {
            OnAlbumSavingFail(_viewModel.Model.ValidationResult);
            return;
        }

        // Execute saving
        var validationResult = await _appService.AddNewAlbumAsync(_viewModel.Model);
        if (validationResult.IsValid)
            await OnAlbumSaved();
        else
            OnAlbumSavingFail(validationResult);
    }
}