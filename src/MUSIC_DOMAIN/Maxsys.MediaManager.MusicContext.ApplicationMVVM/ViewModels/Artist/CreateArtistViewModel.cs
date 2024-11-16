//using CommunityToolkit.Mvvm.Input;
//using Maxsys.MediaManager.CoreDomain.Interfaces;
//using Maxsys.MediaManager.MusicContext.ApplicationMVVM.Interfaces.Services;
//using Maxsys.MediaManager.MusicContext.ApplicationMVVM.Models;
//using Maxsys.MediaManager.MusicContext.ApplicationMVVM.ViewModels.Catalog;
//using Maxsys.ModelCore.Interfaces.Services;
//using Microsoft.Extensions.Logging;
//using System.Collections.Generic;
//using System.Collections.ObjectModel;
//using System.Linq;
//using System.Threading.Tasks;
//using System.Windows.Input;
//using ValidationResult = FluentValidation.Results.ValidationResult;

//namespace Maxsys.MediaManager.MusicContext.ApplicationMVVM.ViewModels
//{
//    public sealed class CreateArtistViewModel : ViewModelBase<CreateArtistModel>
//    {
//        #region FIELDS

//        private readonly ICreateArtistAppService _appService;

//        private ReadOnlyObservableCollection<ArtistInfoModel> _artists;
//        private ReadOnlyObservableCollection<CatalogDetailViewModel> _musicCatalogs;
//        private CatalogDetailViewModel _selectedMusicCatalog;

//        #endregion FIELDS

//        #region PROPS

//        public ReadOnlyObservableCollection<ArtistInfoModel> DisplayableArtists
//            => _artists
//            ?.Where(a => a.MusicCatalogId == SelectedMusicCatalog?.CatalogId)
//            ?.OrderBy(a => a.ArtistName)
//            ?.ToReadOnlyObservableCollection();

//        public ReadOnlyObservableCollection<CatalogDetailViewModel> MusicCatalogs
//        {
//            get => _musicCatalogs;
//            private set => SetProperty(ref _musicCatalogs, value);
//        }

//        public CatalogDetailViewModel SelectedMusicCatalog
//        {
//            get => _selectedMusicCatalog;
//            set
//            {
//                if (SetProperty(ref _selectedMusicCatalog, value))
//                    OnPropertyChanged(nameof(DisplayableArtists));
//            }
//        }

//        #endregion PROPS

//        #region COMMANDS

//        public ICommand SaveCommand { get; }

//        #endregion COMMANDS

//        #region METHODS

//        public override async Task LoadedCatalogsAsync()
//        {
//            await LoadMusicCatalogsAsync();
//            await LoadArtistsAsync();
//        }

//        private async Task LoadMusicCatalogsAsync()
//        {
//            _logger.LogInformation("Loading registered Catalogs.");

//            MusicCatalogs = (await _appService.GetMusicCatalogsAsync()).ToReadOnlyObservableCollection();

//            _logger.LogInformation("Registered Catalogs loaded.");
//        }

//        private async Task LoadArtistsAsync()
//        {
//            _logger.LogInformation("Loading registered Artists.");

//            _artists = (await _appService.GetArtistsAsync()).ToReadOnlyObservableCollection();

//            _logger.LogInformation("Registered Artists loaded.");
//        }

//        private async Task<ValidationResult> SaveAction()
//        {
//            _logger.LogInformation("Registering Artist [{CatalogName}].", Model.Name);

//            Model.SetMusicCatalog(SelectedMusicCatalog);

//            var validationResult = Model.IsValid
//                ? await _appService.AddNewArtistAsync(Model)
//                : await Task.FromResult(Model.ValidationResult);

//            if (validationResult.IsValid)
//                await OnArtistSaved();
//            else
//                OnArtistSaveFail(validationResult);

//            return validationResult;
//        }

//        private async Task OnArtistSaved()
//        {
//            var message = $"Artist [{Model.Name}] registered!";

//            _logger.LogInformation(message);
//            _dialogService.ShowMessage(MessageType.Information, message);

//            Model = new();

//            await LoadArtistsAsync();

//            // UpdateSongRankAsync Artist ListView
//            OnPropertyChanged(nameof(DisplayableArtists));
//        }

//        private void OnArtistSaveFail(ValidationResult validationResult)
//        {
//            var message = $"Error while registering artist:\n{validationResult}";

//            _dialogService.ShowMessage(MessageType.Error, message);
//            _logger.LogError(message);
//        }

//        #endregion METHODS

//        #region CTOR

//        public CreateArtistViewModel(
//            ILogger logger,
//            IDialogService dialogService,
//            IMainContentCloser contentCloser,
//            ICreateArtistAppService appService)
//            : base(logger, dialogService, contentCloser)
//        {
//            _appService = appService;

//            SaveCommand = new AsyncRelayCommand(SaveAction);
//        }

//        #endregion CTOR
//    }
//}