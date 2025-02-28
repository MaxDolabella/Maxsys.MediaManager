using System.Threading.Tasks;
using Maxsys.MediaManager.MusicContext.ApplicationMVVM.Interfaces.Services;
using Maxsys.MediaManager.MusicContext.ApplicationMVVM.Interfaces.ViewModels;
using Maxsys.MediaManager.MusicContext.WPF.Commands.Abstrations;
using Maxsys.MediaManager.MusicContext.WPF.Services;
using Microsoft.Extensions.Logging;

namespace Maxsys.MediaManager.MusicContext.ApplicationMVVM.Commands;

public class CreateCatalogAsyncCommand : AsyncCommandBase
{
    private readonly ILogger _logger;
    private readonly ICatalogCreateViewModel _viewModel;
    private readonly ICatalogCreateAppService _appService;
    private readonly IDialogService _dialogService;

    public CreateCatalogAsyncCommand(
        ILogger<CreateCatalogAsyncCommand> logger,
        ICatalogCreateViewModel viewModel,
        ICatalogCreateAppService appService,
        IDialogService dialogService)
    {
        _logger = logger;
        _viewModel = viewModel;
        _appService = appService;
        _dialogService = dialogService;
    }

    private async Task OnMusicCatalogSaved()
    {
        var message = $"Catalog [{_viewModel.CatalogName}] registered!";

        _logger.LogInformation(message);
        _dialogService.ShowMessage(MessageType.Information, message);

        await _viewModel.LoadCatalogsAsync();
    }

    private void OnMusicCatalogSaveFail(string errors)
    {
        var message = $"Error while registering Song Catalog: {errors}";

        _dialogService.ShowMessage(MessageType.Error, message);
        _logger.LogError(message);
    }

    public override bool CanExecute(object? parameter)
    {
        return !string.IsNullOrWhiteSpace(_viewModel.CatalogName);
    }

    public override async Task ExecuteAsync(object? parameter)
    {
        _logger.LogInformation("Registering Catalog [{CatalogName}].", _viewModel.CatalogName);

        // Validate Model
        if (!_viewModel.IsValid)
        {
            OnMusicCatalogSaveFail(_viewModel.Errors);
            return;
        }

        // Execute saving
        var validationResult = await _appService.CreateCatalogAsync(null);
        if (validationResult.IsValid)
            await OnMusicCatalogSaved();
        else
            OnMusicCatalogSaveFail(validationResult.ToString());
    }
}