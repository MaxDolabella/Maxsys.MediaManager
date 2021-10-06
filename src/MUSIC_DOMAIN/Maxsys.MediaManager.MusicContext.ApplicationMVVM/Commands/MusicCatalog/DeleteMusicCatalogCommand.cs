using FluentValidation.Results;
using Maxsys.MediaManager.CoreDomain.Commands;
using Maxsys.MediaManager.CoreDomain.Interfaces;
using Maxsys.MediaManager.MusicContext.ApplicationMVVM.Interfaces.Services;
using Maxsys.MediaManager.MusicContext.ApplicationMVVM.ViewModels;
using Maxsys.ModelCore.Interfaces.Services;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace Maxsys.MediaManager.MusicContext.ApplicationMVVM.Commands
{
    public class DeleteMusicCatalogCommand : CommandBase
    {
        #region Fields

        private readonly DeleteMusicCatalogViewModel _viewModel;

        private readonly IDeleteMusicCatalogAppService _appService;

        private readonly ILogger _logger;
        private readonly IDialogService _dialogService;
        private readonly IQuestionDialogService _questionDialogService;

        #endregion Fields

        public DeleteMusicCatalogCommand(
            DeleteMusicCatalogViewModel viewModel,
            ILogger logger,
            IQuestionDialogService questionDialogService,
            IDialogService dialogService,
            IDeleteMusicCatalogAppService appService)
        {
            _viewModel = viewModel;
            _logger = logger;
            _dialogService = dialogService;
            _questionDialogService = questionDialogService;
            _appService = appService;

            _viewModel.PropertyChanged += (s, e) =>
            {
                if (e.PropertyName == nameof(_viewModel.SelectedModel))
                    OnCanExecuteChanged();
            };
        }

        private bool ConfirmDeletion()
        {
            var result = _questionDialogService
                .ShowQuestion($"Really wants delete the Music Catalog '{_viewModel.SelectedModel.MusicCatalogName}'?",
                "DELETE MUSIC CATALOG");

            return result == IQuestionDialogService.QuestionDialogResult.Yes;
        }

        private async Task OnMusicCatalogDeleted()
        {
            var message = $"Music Catalog [{_viewModel.SelectedModel.MusicCatalogName}] Deleted!";

            _logger.LogInformation(message);
            _dialogService.ShowMessage(MessageType.Information, message);

            _ = _appService.DeleteMusicCatalogDirectory(_viewModel.SelectedModel)
                .ConfigureAwait(false);

            await _viewModel.ViewLoadedAsync();
        }

        private void OnMusicCatalogDeleteFail(ValidationResult validationResult)
        {
            var message = $"Error while deleting music catalog:\n{validationResult}";

            _logger.LogError(message);
            _dialogService.ShowMessage(MessageType.Error, message);
        }

        public override bool CanExecute(object parameter)
        {
            return _viewModel.SelectedModel is not null
                && _viewModel.SelectedModel.ArtistCount == 0
                && base.CanExecute(parameter);
        }

        public override async void Execute(object parameter)
        {
            if (ConfirmDeletion())
            {
                _logger.LogInformation($"Deleting music catalog [{_viewModel.SelectedModel.MusicCatalogName}]...");

                // Execute deletion
                var validationResult = await _appService.DeleteMusicCatalogAsync(_viewModel.SelectedModel);
                if (validationResult.IsValid)
                    await OnMusicCatalogDeleted();
                else
                    OnMusicCatalogDeleteFail(validationResult);
            }
        }
    }
}