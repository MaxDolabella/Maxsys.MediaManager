using FluentValidation.Results;
using Maxsys.MediaManager.CoreDomain.Interfaces;
using Maxsys.MediaManager.MusicContext.ApplicationMVVM.Interfaces.Services;
using Maxsys.MediaManager.MusicContext.ApplicationMVVM.Models;
using Maxsys.MediaManager.MusicContext.ApplicationMVVM.ViewModels;
using Maxsys.ModelCore.Interfaces.Services;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Maxsys.MediaManager.MusicContext.ApplicationMVVM.Commands
{
    public class DeleteMusicCommand : CommandBase
    {
        //public event EventHandler CanExecuteChanged;

        private readonly MusicsViewModel _viewModel;
        private readonly ILogger _logger;
        private readonly IMusicListAppService _appService;
        private readonly IDialogService _dialogService;
        private readonly IQuestionDialogService _questionDialogService;
        MusicListModel SelectedMusic => _viewModel.SelectedMusic;

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
                if (e.PropertyName == nameof(SelectedMusic))
                    OnCanExecuteChanged();
            };
        }

        public override bool CanExecute(object parameter)
        {
            return SelectedMusic is not null 
                && base.CanExecute(parameter);
        }

        public override async void Execute(object parameter)
        {
            var result = _questionDialogService
                .ShowQuestion($"Really wants delete the music '{SelectedMusic.MusicTitle}'?",
                "DELETE MUSIC");

            if (result == IQuestionDialogService.QuestionDialogResult.Yes)
            {
                _logger.LogDebug($"Deleting music '{SelectedMusic.MusicTitle}'.");

                var validationResult = await _appService.DeleteMusicAsync(SelectedMusic);

                if (validationResult.IsValid)
                    await OnMusicDeleted();
                else
                    OnMusicDeleteFail(validationResult);
            }
        }

        private async Task OnMusicDeleted()
        {
            var message = $"Music [{SelectedMusic.MusicTitle}] Deleted!";

            _logger.LogInformation(message);
            _dialogService.ShowMessage(MessageType.Information, message);

            _ = _appService.DeleteMusicFileAsync(SelectedMusic)
                .ConfigureAwait(false);

            await _viewModel.ViewLoadedAsync();
        }

        private void OnMusicDeleteFail(ValidationResult validationResult)
        {
            var message = $"Error while deleting music catalog:\n{validationResult}";

            _logger.LogError(message);
            _dialogService.ShowMessage(MessageType.Error, message);
        }
    }
}