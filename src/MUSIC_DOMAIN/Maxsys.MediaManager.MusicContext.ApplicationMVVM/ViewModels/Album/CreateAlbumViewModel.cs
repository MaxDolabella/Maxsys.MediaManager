using CommunityToolkit.Mvvm.Input;
using Maxsys.Core.Helpers;
using Maxsys.MediaManager.CoreDomain.Interfaces;
using Maxsys.MediaManager.CoreDomain.Properties;
using Maxsys.MediaManager.MusicContext.ApplicationMVVM.Interfaces.Services;
using Maxsys.MediaManager.MusicContext.ApplicationMVVM.Models;
using Maxsys.MediaManager.MusicContext.Domain.DTO;
using Maxsys.ModelCore.Interfaces.Services;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using ValidationResult = FluentValidation.Results.ValidationResult;

namespace Maxsys.MediaManager.MusicContext.ApplicationMVVM.ViewModels
{
    public sealed class CreateAlbumViewModel : ViewModelBase<CreateAlbumModel>
    {
        #region FIELDS

        private readonly ICreateAlbumAppService _appService;

        private ReadOnlyObservableCollection<string> _genres;
        private ReadOnlyObservableCollection<ArtistInfoDTO> _artists;
        private ReadOnlyObservableCollection<AlbumInfoDTO> _albums;
        private ArtistInfoDTO _selectedArtist;

        #endregion FIELDS

        #region PROPS

        public ReadOnlyObservableCollection<AlbumInfoDTO> DisplayableAlbums
            => _albums
            ?.Where(a => a.ArtistId == SelectedArtist?.ArtistId)
            ?.OrderBy(a => a.AlbumDirectory)
            ?.ToReadOnlyObservableCollection();

        public ReadOnlyObservableCollection<ArtistInfoDTO> Artists
        {
            get => _artists;
            private set => SetProperty(ref _artists, value);
        }

        public ReadOnlyObservableCollection<string> Genres
        {
            get => _genres;
            private set => SetProperty(ref _genres, value);
        }

        public ArtistInfoDTO SelectedArtist
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
        public ICommand CloseCommand { get; }

        #endregion COMMANDS

        #region METHODS

        public override async Task ViewLoadedAsync()
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

        private async Task<ValidationResult> SaveAction()
        {
            _logger.LogInformation($"Registering Album [{Model.Name}]...");

            Model.SetArtist(SelectedArtist);

            var validationResult = Model.IsValid
                ? await _appService.AddNewAlbumAsync(Model)
                : await Task.FromResult(Model.ValidationResult);

            if (validationResult.IsValid)
                await OnAlbumSaved();
            else
                OnAlbumSaveFailed(validationResult);

            return validationResult;
        }

        private async Task OnAlbumSaved()
        {
            var message = $"Album [{Model.Name}] registered.";

            _logger.LogInformation(message);
            _dialogService.ShowMessage(MessageType.Information, message);

            Model = new();

            await ViewLoadedAsync();

            OnPropertyChanged(nameof(DisplayableAlbums));
        }

        private void OnAlbumSaveFailed(ValidationResult validationResult)
        {
            var message = $"Error while registering album:\n{validationResult}";

            _dialogService.ShowMessage(MessageType.Error, message);
            _logger.LogError(message);
        }

        private void CloseAction()
        {
            _mainContentCloser.CloseMainContent();
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

            SaveCommand = new AsyncRelayCommand(SaveAction);
            CloseCommand = new RelayCommand(CloseAction);
        }

        #endregion CTOR
    }
}