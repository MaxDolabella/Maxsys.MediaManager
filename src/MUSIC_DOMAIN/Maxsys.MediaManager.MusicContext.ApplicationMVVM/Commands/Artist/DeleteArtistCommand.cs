using FluentValidation.Results;
using Maxsys.MediaManager.CoreDomain.Commands;
using Maxsys.MediaManager.CoreDomain.Interfaces;
using Maxsys.MediaManager.MusicContext.ApplicationMVVM.Interfaces.Services;
using Maxsys.MediaManager.MusicContext.ApplicationMVVM.ViewModels;
using Maxsys.MediaManager.MusicContext.Domain.DTO;
using Maxsys.MediaManager.MusicContext.Domain.Interfaces.Services;
using Maxsys.ModelCore.Interfaces.Services;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace Maxsys.MediaManager.MusicContext.ApplicationMVVM.Commands
{
    public class DeleteArtistCommand : CommandBase
    {
        private readonly IDeleteArtistAppService _appService;
        private readonly DeleteArtistViewModel _viewModel;

        private readonly ILogger _logger;
        private readonly IDialogService _dialogService;
        private readonly IQuestionDialogService _questionDialogService;
        private readonly IPathService _pathService;

        public DeleteArtistCommand(
            DeleteArtistViewModel viewModel,
            ILogger logger,
            IQuestionDialogService questionDialogService,
            IDialogService dialogService,
            IDeleteArtistAppService appService,
            IPathService pathService)
        {
            _viewModel = viewModel;
            _logger = logger;
            _dialogService = dialogService;
            _questionDialogService = questionDialogService;
            _appService = appService;
            _pathService = pathService;

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
                .ShowQuestion($"Really wants delete the artist '{_viewModel.SelectedModel.ArtistName}'?",
                "DELETE ARTIST");

            return result == IQuestionDialogService.QuestionDialogResult.Yes;
        }

        private async Task DeleteArtistDirectory()
        {
            await Task.Run(() =>
            {
                try
                {
                    var dto = new DefineArtistFolderDTO
                    {
                        ArtistName = _viewModel.SelectedModel.ArtistName,
                        MusicCatalogName = _viewModel.SelectedModel.MusicCatalogName
                    };

                    var artistDirectory = _pathService.GetArtistDirectory(dto);

                    _logger.LogDebug($"Deleting folder <{artistDirectory}>.");

                    System.IO.Directory.Delete(artistDirectory, true);

                    _logger.LogWarning($"Folder deleted.");
                }
                catch (System.Exception ex)
                {
                    _logger.LogError("Folder cannot be deleted: {errors}", ex.Message);
                }
            });
        }

        private async Task OnArtistDeleted()
        {
            var message = $"Artist [{_viewModel.SelectedModel.ArtistName}] Deleted!";

            _logger.LogInformation(message);
            _dialogService.ShowMessage(MessageType.Information, message);

            _ = DeleteArtistDirectory().ConfigureAwait(false);

            await _viewModel.LoadedCatalogsAsync();
        }

        private void OnArtistDeleteFail(ValidationResult validationResult)
        {
            var message = $"Error while deleting artist:\n{validationResult}";

            _logger.LogError(message);
            _dialogService.ShowMessage(MessageType.Error, message);
        }

        #endregion Private Methods

        #region ICommand

        public override bool CanExecute(object parameter)
        {
            return _viewModel.SelectedModel is not null
                && _viewModel.SelectedModel.AlbumsCount == 0
                && base.CanExecute(parameter);
        }

        public override async void Execute(object parameter)
        {
            if (ConfirmDeletion())
            {
                _logger.LogInformation($"Deleting artist [{_viewModel.SelectedModel.ArtistName}]...");

                // Execute deletion
                var validationResult = await _appService.DeleteArtistAsync(_viewModel.SelectedModel);
                if (validationResult.IsValid)
                    await OnArtistDeleted();
                else
                    OnArtistDeleteFail(validationResult);
            }
        }

        #endregion ICommand
    }
}