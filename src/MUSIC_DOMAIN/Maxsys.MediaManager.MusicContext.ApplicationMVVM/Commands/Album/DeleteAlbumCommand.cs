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
    public class DeleteAlbumCommand : CommandBase
    {
        private readonly IDeleteAlbumAppService _appService;
        private readonly DeleteAlbumViewModel _viewModel;

        private readonly ILogger _logger;
        private readonly IDialogService _dialogService;
        private readonly IQuestionDialogService _questionDialogService;

        public DeleteAlbumCommand(
            DeleteAlbumViewModel viewModel,
            ILogger logger,
            IQuestionDialogService questionDialogService,
            IDialogService dialogService,
            IDeleteAlbumAppService appService)
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

        #region Private Methods

        private bool ConfirmDeletion()
        {
            var result = _questionDialogService
                .ShowQuestion($"Really wants delete the album '{_viewModel.SelectedModel.AlbumName}'?",
                "DELETE ALBUM");

            return result == IQuestionDialogService.QuestionDialogResult.Yes;
        }

        private async Task DeleteAlbumDirectory()
        {
            await Task.Run(() =>
            {
                try
                {
                    _logger.LogDebug($"Deleting folder <{_viewModel.SelectedModel.AlbumDirectory}>.");

                    System.IO.Directory.Delete(_viewModel.SelectedModel.AlbumDirectory, true);

                    _logger.LogWarning($"Folder deleted.");
                }
                catch (System.Exception ex)
                {
                    _logger.LogError("Folder cannot be deleted: {errors}", ex.Message);
                }
            });
        }

        private async Task OnAlbumDeleted()
        {
            var message = $"Album [{_viewModel.SelectedModel.AlbumName}] deleted!";

            _logger.LogInformation(message);
            _dialogService.ShowMessage(MessageType.Information, message);

            _ = DeleteAlbumDirectory().ConfigureAwait(false);

            await _viewModel.ViewLoadedAsync();
        }

        private void OnAlbumDeleteFail(ValidationResult validationResult)
        {
            var message = $"Error while deleting album:\n{validationResult}";

            _logger.LogError(message);
            _dialogService.ShowMessage(MessageType.Error, message);
        }

        #endregion Private Methods

        #region ICommand

        public override bool CanExecute(object parameter)
        {
            return _viewModel.SelectedModel is not null
                && _viewModel.SelectedModel.AlbumMusicCount == 0
                && base.CanExecute(parameter);
        }

        public override async void Execute(object parameter)
        {
            if (ConfirmDeletion())
            {
                _logger.LogInformation($"Deleting album [{_viewModel.SelectedModel.AlbumName}]...");

                var validationResult = await _appService.DeleteAlbumAsync(_viewModel.SelectedModel);

                if (validationResult.IsValid)
                    await OnAlbumDeleted();
                else
                    OnAlbumDeleteFail(validationResult);
            }
        }

        #endregion ICommand
    }
}