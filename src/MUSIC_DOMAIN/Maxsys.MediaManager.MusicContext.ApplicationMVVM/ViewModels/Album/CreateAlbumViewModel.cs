using Maxsys.Core.Helpers;
using Maxsys.MediaManager.CoreDomain.Interfaces;
using Maxsys.MediaManager.CoreDomain.Properties;
using Maxsys.MediaManager.MusicContext.ApplicationMVVM.Commands;
using Maxsys.MediaManager.MusicContext.ApplicationMVVM.Interfaces.Services;
using Maxsys.MediaManager.MusicContext.ApplicationMVVM.Models;
using Maxsys.ModelCore.Interfaces.Services;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Maxsys.MediaManager.MusicContext.ApplicationMVVM.ViewModels
{
    public sealed class CreateAlbumViewModel : ViewModelBase<CreateAlbumModel>
    {
        #region FIELDS

        private readonly ICreateAlbumAppService _appService;

        private ReadOnlyObservableCollection<string> _genres;
        private ReadOnlyObservableCollection<ArtistInfoModel> _artists;
        private ReadOnlyObservableCollection<AlbumInfoModel> _albums;
        private ArtistInfoModel _selectedArtist;

        #endregion FIELDS

        #region PROPS

        public ReadOnlyObservableCollection<AlbumInfoModel> DisplayableAlbums
            => _albums
            ?.Where(a => a.ArtistId == SelectedArtist?.ArtistId)
            ?.OrderBy(a => a.AlbumDirectory)
            ?.ToReadOnlyObservableCollection();

        public ReadOnlyObservableCollection<ArtistInfoModel> Artists
        {
            get => _artists;
            private set => SetProperty(ref _artists, value);
        }

        public ReadOnlyObservableCollection<string> Genres
        {
            get => _genres;
            private set => SetProperty(ref _genres, value);
        }

        public ArtistInfoModel SelectedArtist
        {
            get => _selectedArtist;
            set
            {
                if (SetProperty(ref _selectedArtist, value))
                    OnPropertyChanged(nameof(DisplayableAlbums));
            }
        }

        #endregion PROPS

        #region COMMANDS

        public ICommand SaveCommand { get; }
        public ICommand AlbumCoverDropCommand { get; }

        #endregion COMMANDS

        #region METHODS

        public override async Task LoadedCatalogsAsync()
        {
            LoadDefaultImage();
            await LoadGenresAsync();
            await LoadAlbumsAsync();
            await LoadArtistsAsync();
        }

        private void LoadDefaultImage()
        {
            Model.AlbumCover = Resources.DEFAULT_ALBUM_COVER.ImageToBytes();
        }

        private async Task LoadArtistsAsync()
        {
            _logger.LogDebug("Loading registered Artists.");

            Artists = (await _appService.GetArtistsAsync()).ToReadOnlyObservableCollection();

            _logger.LogDebug("Registered Artists loaded.");
        }

        private async Task LoadGenresAsync()
        {
            _logger.LogDebug("Loading registered Genres.");

            Genres = (await _appService.GetGenresAsync()).ToReadOnlyObservableCollection();

            _logger.LogDebug("Registered Genres loaded.");
        }

        private async Task LoadAlbumsAsync()
        {
            _logger.LogDebug("Loading registered Albums.");

            _albums = (await _appService.GetAlbumsAsync()).ToReadOnlyObservableCollection();

            _logger.LogDebug("Registered Albums loaded.");
        }

        #endregion METHODS

        #region CTOR

        public CreateAlbumViewModel(
            ILogger logger,
            IDialogService dialogService,
            IMainContentCloser contentCloser,
            ICreateAlbumAppService appService)
            : base(logger, dialogService, contentCloser)
        {
            _appService = appService;

            SaveCommand = new CreateAlbumCommand(this, logger, appService, dialogService);
            AlbumCoverDropCommand = new AlbumCoverDropCommand(this, dialogService);
        }

        #endregion CTOR
    }
}