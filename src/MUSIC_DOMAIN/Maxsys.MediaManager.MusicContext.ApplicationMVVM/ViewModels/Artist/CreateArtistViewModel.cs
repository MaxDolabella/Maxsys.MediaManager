using CommunityToolkit.Mvvm.Input;
using Maxsys.MediaManager.CoreDomain.Interfaces;
using Maxsys.MediaManager.MusicContext.ApplicationMVVM.Interfaces.Services;
using Maxsys.MediaManager.MusicContext.ApplicationMVVM.Models;
using Maxsys.MediaManager.MusicContext.Domain.DTO;
using Maxsys.ModelCore.Interfaces.Services;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using ValidationResult = FluentValidation.Results.ValidationResult;

namespace Maxsys.MediaManager.MusicContext.ApplicationMVVM.ViewModels
{
    public sealed class CreateArtistViewModel : ViewModelBase<CreateArtistModel>
    {
        #region FIELDS

        private readonly ICreateArtistAppService _appService;

        private ReadOnlyObservableCollection<ArtistInfoDTO> _artists;
        private ReadOnlyObservableCollection<MusicCatalogInfoDTO> _musicCatalogs;
        private MusicCatalogInfoDTO _selectedMusicCatalog;

        #endregion FIELDS

        #region PROPS

        public ReadOnlyObservableCollection<ArtistInfoDTO> DisplayableArtists
            => _artists
            ?.Where(a => a.MusicCatalogId == SelectedMusicCatalog?.MusicCatalogId)
            ?.OrderBy(a => a.ArtistName)
            ?.ToReadOnlyObservableCollection();

        public ReadOnlyObservableCollection<MusicCatalogInfoDTO> MusicCatalogs
        {
            get => _musicCatalogs;
            private set => SetProperty(ref _musicCatalogs, value);
        }

        public MusicCatalogInfoDTO SelectedMusicCatalog
        {
            get => _selectedMusicCatalog;
            set
            {
                if (SetProperty(ref _selectedMusicCatalog, value))
                    OnPropertyChanged(nameof(DisplayableArtists));
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
            await LoadMusicCatalogsAsync();
            await LoadArtistsAsync();
        }

        private async Task LoadMusicCatalogsAsync()
        {
            _logger.LogInformation("Loading registered MusicCatalogs.");

            MusicCatalogs = (await _appService.GetMusicCatalogsAsync()).ToReadOnlyObservableCollection();

            //SelectedMusicCatalog = MusicCatalogs?.FirstOrDefault();

            _logger.LogInformation("Registered MusicCatalogs loaded.");
        }

        private async Task LoadArtistsAsync()
        {
            _logger.LogInformation("Loading registered Artists.");

            _artists = (await _appService.GetArtistsAsync()).ToReadOnlyObservableCollection();

            _logger.LogInformation("Registered Artists loaded.");
        }

        private async Task<ValidationResult> SaveAction()
        {
            _logger.LogInformation("Registering Artist [{Name}].", Model.Name);

            Model.MusicCatalogId = SelectedMusicCatalog.MusicCatalogId;

            var validationResult = Model.IsValid
                ? await _appService.AddNewArtistAsync(Model)
                : Model.ValidationResult;

            if (validationResult.IsValid)
            {
                _logger.LogInformation("Artist [{Name}] registered.", Model.Name);
                _dialogService.ShowMessage(MessageType.Information, $"Artist [{Model.Name}] saved!");

                await OnArtistSaved();
            }
            else
            {
                var errors = validationResult.ToString(Environment.NewLine);
                _dialogService.ShowMessage(MessageType.Error, $"Error at registering Artist: {errors}");
                _logger.LogError("Error at registering Artist [{Name}]: {Errors}"
                    , Model.Name, errors);
            }

            return await Task.FromResult(validationResult);
        }

        private async Task OnArtistSaved()
        {
            Model = new();

            await LoadArtistsAsync();

            // Update Artist ListView
            OnPropertyChanged(nameof(DisplayableArtists));
        }

        private void CloseAction()
        {
            _mainContentCloser.CloseMainContent();
        }

        #endregion METHODS

        #region CTOR

        public CreateArtistViewModel(
            ILogger logger,
            IDialogService dialogService,
            IMainContentCloser contentCloser,
            ICreateArtistAppService appService)
            : base(logger, dialogService, contentCloser)
        {
            _appService = appService;

            SaveCommand = new AsyncRelayCommand(SaveAction);
            CloseCommand = new RelayCommand(CloseAction);
        }

        #endregion CTOR
    }
}