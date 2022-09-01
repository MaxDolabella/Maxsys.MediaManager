using Maxsys.MediaManager.CoreDomain.Interfaces;
using Maxsys.MediaManager.MusicContext.ApplicationMVVM.Commands;
using Maxsys.MediaManager.MusicContext.ApplicationMVVM.Interfaces.Services;
using Maxsys.MediaManager.MusicContext.ApplicationMVVM.Models;
using Maxsys.MediaManager.MusicContext.Domain.Interfaces.Services;
using Maxsys.ModelCore.Interfaces.Services;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Maxsys.MediaManager.MusicContext.ApplicationMVVM.ViewModels
{
    public sealed class DeleteArtistViewModel : ViewModelCollectionBase<ArtistListModel>
    {
        #region CTOR

        public DeleteArtistViewModel(
            ILogger logger,
            IDialogService dialogService,
            IQuestionDialogService questionDialogService,
            IMainContentCloser contentCloser,
            IPathService pathService,
            IDeleteArtistAppService appService)
            : base(logger, dialogService, contentCloser)
        {
            _appService = appService;

            DeleteArtistCommand = new DeleteArtistCommand(this, logger, questionDialogService, dialogService, appService, pathService);
        }

        #endregion CTOR

        #region FIELDS

        private readonly IDeleteArtistAppService _appService;

        private ArtistListModel _selectedModel;

        #endregion FIELDS

        #region PROPS

        public ArtistListModel SelectedModel
        {
            get => _selectedModel;
            set => SetProperty(ref _selectedModel, value);
        }

        #endregion PROPS

        #region COMMANDS

        public ICommand DeleteArtistCommand { get; }

        #endregion COMMANDS

        #region METHODS

        public override async Task LoadedCatalogsAsync()
        {
            await LoadArtistsAsync();
        }

        private async Task LoadArtistsAsync()
        {
            _logger.LogDebug("Loading Artists.");

            Models = (await _appService.GetArtistsAsync()).ToObservableCollection();

            _logger.LogDebug("Artists loaded.");
        }

        #endregion METHODS
    }
}