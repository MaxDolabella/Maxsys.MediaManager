using Maxsys.MediaManager.CoreDomain.Interfaces;
using Maxsys.MediaManager.MusicContext.ApplicationMVVM.Commands;
using Maxsys.MediaManager.MusicContext.ApplicationMVVM.Interfaces.Services;
using Maxsys.MediaManager.MusicContext.ApplicationMVVM.Models;
using Maxsys.ModelCore.Interfaces.Services;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Maxsys.MediaManager.MusicContext.ApplicationMVVM.ViewModels
{
    public class CreateMusicCatalogViewModel : ViewModelBase<CreateMusicCatalogModel>
    {
        private readonly ICreateMusicCatalogAppService _appService;

        #region CTOR

        public CreateMusicCatalogViewModel(
            ILogger logger,
            IDialogService dialogService,
            IMainContentCloser contentCloser,
            ICreateMusicCatalogAppService appService)
            : base(logger, dialogService, contentCloser)
        {
            _appService = appService;

            SaveCommand = new CreateMusicCatalogCommand(logger, this, appService, dialogService);
        }

        #endregion CTOR

        #region PROPS

        private ReadOnlyObservableCollection<MusicCatalogInfoModel> _musicCatalogs;

        public ReadOnlyObservableCollection<MusicCatalogInfoModel> MusicCatalogs
        {
            get => _musicCatalogs;
            private set => SetProperty(ref _musicCatalogs, value);
        }

        #endregion PROPS

        #region COMMANDS

        public ICommand SaveCommand { get; }

        #endregion COMMANDS

        #region METHODS

        public override async Task ViewLoadedAsync()
        {
            await LoadMusicCatalogsAsync();
        }

        private async Task LoadMusicCatalogsAsync()
        {
            _logger.LogDebug("Loading registered MusicCatalogs.");

            MusicCatalogs = (await _appService.GetMusicCatalogsAsync()).ToReadOnlyObservableCollection();

            _logger.LogDebug("Registered MusicCatalogs loaded.");
        }

        #endregion METHODS
    }
}