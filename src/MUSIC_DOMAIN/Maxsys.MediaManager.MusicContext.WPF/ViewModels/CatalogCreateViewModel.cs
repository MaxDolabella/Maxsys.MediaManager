using Maxsys.MediaManager.CoreDomain.Commands;
using Maxsys.MediaManager.MusicContext.ApplicationMVVM.Commands;
using Maxsys.MediaManager.MusicContext.ApplicationMVVM.Interfaces.Services;
using Maxsys.MediaManager.MusicContext.ApplicationMVVM.Interfaces.ViewModels;
using Maxsys.MediaManager.MusicContext.ApplicationMVVM.ViewModels.Abstractions;
using Maxsys.MediaManager.MusicContext.ApplicationMVVM.ViewModels.Catalog;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Maxsys.MediaManager.MusicContext.ApplicationMVVM.ViewModels
{
    public class CatalogCreateViewModel : ValidableViewModelBase, ICatalogCreateViewModel
    {
        private readonly ILogger _logger;
        private readonly ICatalogCreateAppService _appService;

        #region CTOR

        public CatalogCreateViewModel(
            ILogger<CatalogCreateViewModel> logger,
            ICatalogCreateAppService appService,
            CreateCatalogAsyncCommand saveCommand)
        {
            _logger = logger;
            _appService = appService;

            SaveCommand = saveCommand;
        }

        #endregion CTOR

        #region PROPS

        private IReadOnlyList<CatalogDetailViewModel> _catalogs;
        private string _catalogName;

        public IReadOnlyList<CatalogDetailViewModel> Catalogs
        {
            get => _catalogs;
            private set => SetProperty(ref _catalogs, value);
        }

        public string CatalogName
        {
            get => _catalogName;
            private set => SetProperty(ref _catalogName, value);
        }

        #endregion PROPS

        #region COMMANDS

        public IAsyncCommand SaveCommand { get; }

        #endregion COMMANDS

        #region METHODS

        public async Task LoadCatalogsAsync(CancellationToken cancellation = default)
        {
            _logger.LogDebug("Loading registered Catalogs.");

            Catalogs = (await _appService.GetMusicCatalogsAsync()).ToReadOnlyObservableCollection();

            _logger.LogDebug("Registered Catalogs loaded.");
        }

        #endregion METHODS
    }
}