using FluentValidation.Results;
using Maxsys.MediaManager.CoreDomain.Commands;
using Maxsys.MediaManager.CoreDomain.Interfaces;
using Maxsys.MediaManager.MusicContext.ApplicationMVVM.Interfaces.Services;
using Maxsys.MediaManager.MusicContext.ApplicationMVVM.Models;
using Maxsys.MediaManager.MusicContext.ApplicationMVVM.ViewModels;
using Maxsys.ModelCore.Interfaces.Services;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace Maxsys.MediaManager.MusicContext.ApplicationMVVM.Commands
{
    public class DeleteMusicCommand : CommandBase
    {
        #region Fields

        private readonly MusicsViewModel _viewModel;
        private readonly ILogger _logger;
        private readonly IMusicListAppService _appService;
        private readonly IDialogService _dialogService;
        private readonly IQuestionDialogService _questionDialogService;

        #endregion
        MusicListModel Model => _viewModel.SelectedMusic;

        public DeleteMusicCommand(
            MusicsViewModel viewModel,
            ILogger logger,
            IQuestionDialogService questionDialogService,
            IDialogService dialogService,
            IMusicListAppService appService)
        {
            _logger = logger;
            _viewModel = viewModel;
            _appService = appService;
            _dialogService = dialogService;
            _questionDialogService = questionDialogService;

            _viewModel.PropertyChanged += (s, e) =>
            {
                if (e.PropertyName == nameof(_viewModel.SelectedMusic))
                    OnCanExecuteChanged();
            };
        }

        #region ICommand

        public override bool CanExecute(object parameter)
        {
            return Model is not null
                && base.CanExecute(parameter);
        }

        public override async void Execute(object parameter)
        {
            if (ConfirmDeletion())
            {
                _logger.LogDebug($"Deleting music '{Model.MusicTitle}'.");

                var validationResult = await _appService.DeleteMusicAsync(Model);

                if (validationResult.IsValid)
                    await OnMusicDeleted();
                else
                    OnMusicDeleteFail(validationResult);
            }
        }

        #endregion ICommand

        #region Private Methods

        private bool ConfirmDeletion()
        {
            var result = _questionDialogService
                .ShowQuestion($"Really wants delete the music '{Model.MusicTitle}'?",
                "DELETE MUSIC");

            return result == IQuestionDialogService.QuestionDialogResult.Yes;
        }

        private async Task OnMusicDeleted()
        {
            var message = $"Music [{Model.MusicTitle}] Deleted!";

            _logger.LogInformation(message);
            _dialogService.ShowMessage(MessageType.Information, message);

            _ = _appService.DeleteMusicFileAsync(Model)
                .ConfigureAwait(false);

            await _viewModel.ViewLoadedAsync();
            _viewModel.SearchCommand.Execute(null);
        }

        private void OnMusicDeleteFail(ValidationResult validationResult)
        {
            var message = $"Error while deleting music catalog:\n{validationResult}";

            _logger.LogError(message);
            _dialogService.ShowMessage(MessageType.Error, message);
        }

        #endregion Private Methods
    }
}