using CommunityToolkit.Mvvm.Input;
using FluentValidation.Results;
using Maxsys.MediaManager.CoreDomain.Interfaces;
using Maxsys.MediaManager.MusicContext.ApplicationMVVM.Interfaces.Services;
using Maxsys.MediaManager.MusicContext.ApplicationMVVM.Models;
using Maxsys.MediaManager.MusicContext.Domain.DTO;
using Maxsys.MediaManager.MusicContext.Domain.ValueObjects;
using Maxsys.ModelCore.Interfaces.Services;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using ValidationResult = FluentValidation.Results.ValidationResult;

namespace Maxsys.MediaManager.MusicContext.ApplicationMVVM.ViewModels
{
    public sealed class RegisterMusicViewModel : ViewModelCollectionBase<CreateMusicModel>
    {
        #region FIELDS

        private readonly IRegisterMusicAppService _appService;

        private readonly static ReadOnlyObservableCollection<CreateMusicModel> EMPTY_COLLECTION
            = Array.Empty<CreateMusicModel>().ToReadOnlyObservableCollection();

        private ReadOnlyObservableCollection<MusicCatalogInfoDTO> _musicCatalogs;
        private ReadOnlyObservableCollection<ArtistInfoDTO> _artists;
        private ReadOnlyObservableCollection<AlbumInfoDTO> _albums;
        private ReadOnlyObservableCollection<MusicInfoDTO> _musics;
        private ReadOnlyObservableCollection<CreateMusicModel> _selectedModels;
        private MusicCatalogInfoDTO _selectedMusicCatalog;
        private ArtistInfoDTO _selectedArtist;
        private AlbumInfoDTO _selectedAlbum;
        private VocalGender _selectedVocalGender;

        private int _titleTrimCharCount;
        private string _titlePatternText;
        private int _titlePatternIndex;

        #endregion FIELDS

        #region PROPS

        public IReadOnlyList<VocalGender> VocalGenders { get; }
            = new ReadOnlyCollection<VocalGender>(Enum.GetValues<VocalGender>());

        public ReadOnlyObservableCollection<MusicCatalogInfoDTO> MusicCatalogs
        {
            get => _musicCatalogs;
            private set => SetProperty(ref _musicCatalogs, value);
        }

        public ReadOnlyObservableCollection<ArtistInfoDTO> DisplayableArtists
            => _artists
            ?.Where(a => a.MusicCatalogId == SelectedMusicCatalog.MusicCatalogId)
            ?.OrderBy(a => a.ArtistName)
            ?.ToReadOnlyObservableCollection();

        public ReadOnlyObservableCollection<AlbumInfoDTO> DisplayableAlbums
            => _albums
            ?.Where(a => a.ArtistId == SelectedArtist?.ArtistId)
            ?.OrderBy(a => a.AlbumDirectory)
            ?.ToReadOnlyObservableCollection();

        public ReadOnlyObservableCollection<MusicInfoDTO> DisplayableMusics
            => _musics
            ?.Where(m => m.AlbumId == SelectedAlbum?.AlbumId)
            ?.OrderBy(m => m.MusicTrack)
            ?.ThenBy(m => m.MusicTitle)
            ?.ToReadOnlyObservableCollection();

        public ReadOnlyObservableCollection<CreateMusicModel> SelectedModels
        {
            get => _selectedModels ?? EMPTY_COLLECTION;
            set => SetProperty(ref _selectedModels, value);
        }

        public MusicCatalogInfoDTO SelectedMusicCatalog
        {
            get => _selectedMusicCatalog;
            set
            {
                if (SetProperty(ref _selectedMusicCatalog, value))
                {
                    OnPropertyChanged(nameof(DisplayableArtists));
                    SelectedArtist = DisplayableArtists?.FirstOrDefault();
                }
            }
        }

        public ArtistInfoDTO SelectedArtist
        {
            get => _selectedArtist;
            set
            {
                if (SetProperty(ref _selectedArtist, value))
                {
                    OnPropertyChanged(nameof(DisplayableAlbums));
                    SelectedAlbum = DisplayableAlbums?.FirstOrDefault();
                }
            }
        }

        public AlbumInfoDTO SelectedAlbum
        {
            get => _selectedAlbum;
            set
            {
                if (SetProperty(ref _selectedAlbum, value))
                {
                    OnPropertyChanged(nameof(DisplayableMusics));
                }
            }
        }

        public VocalGender SelectedVocalGender
        {
            get => _selectedVocalGender;
            set => SetProperty(ref _selectedVocalGender, value);
        }

        public int TitleTrimCharCount
        {
            get => _titleTrimCharCount;
            set => SetProperty(ref _titleTrimCharCount, value);
        }

        public string TitlePatternText
        {
            get => _titlePatternText;
            set => SetProperty(ref _titlePatternText, value);
        }

        public int TitlePatternIndex
        {
            get => _titlePatternIndex;
            set => SetProperty(ref _titlePatternIndex, value);
        }

        #endregion PROPS

        #region COMMANDS

        public ICommand SaveCommand { get; }
        public ICommand CloseCommand { get; }

        public ICommand TrimTitleStartCommand { get; }
        public ICommand TrimTitleEndCommand { get; }
        public ICommand IncreaseTitleTrimCharCountCommand { get; }
        public ICommand DecreaseTitleTrimCharCountCommand { get; }      
        public ICommand IncreaseTitlePatternIndexCommand { get; }
        public ICommand DecreaseTitlePatternIndexCommand { get; }
        public ICommand RemovePatternCommand { get; }
        public ICommand InsertPatternStartCommand { get; }
        public ICommand InsertPatternEndCommand { get; }
        public ICommand InsertPatternIndexCommand { get; }
        public ICommand RemoveTrackNumberCommand { get; }
        public ICommand InsertTrackNumberCommand { get; }

        public ICommand SetVocalGenderCommand { get; }
        public ICommand SetAlbumCommand { get; }

        #endregion COMMANDS

        #region METHODS

        public override async Task ViewLoadedAsync()
        {
            await LoadMainComboBoxesActionAsync();

            SelectedMusicCatalog = MusicCatalogs?.FirstOrDefault();
        }

        public void AddMp3FilesAction(string[] paths)
        {
            var isFolder = File.GetAttributes(paths[0]).HasFlag(FileAttributes.Directory);

            var mp3Files = isFolder
                ? Directory.GetFiles(paths[0], "*.mp3")
                : paths.Where(file => Path.GetExtension(file).ToLower() == ".mp3");

            var mp3FilesAlreadyReaded = Models.Select(model => model.SourceFullPath);

            var newModels = mp3Files
                .Where(mp3File => !mp3FilesAlreadyReaded.Contains(mp3File))
                .Select(mp3File => new CreateMusicModel
                {
                    SourceFullPath = mp3File,
                    Title = Path.GetFileNameWithoutExtension(mp3File)
                });

            foreach (var newModel in newModels)
                Models.Add(newModel);
        }

        private async Task LoadMainComboBoxesActionAsync()
        {
            _logger.LogDebug("Loading comboBoxes...");

            // Get lists
            MusicCatalogs = (await _appService.GetMusicCatalogsAsync()).ToReadOnlyObservableCollection();
            _artists = (await _appService.GetArtistsAsync()).ToReadOnlyObservableCollection();
            _albums = (await _appService.GetAlbumsAsync()).ToReadOnlyObservableCollection();
            _musics = (await _appService.GetMusicsAsync()).ToReadOnlyObservableCollection();

            _logger.LogDebug("ComboBoxes loaded.");
        }

        private void CloseAction()
        {
            _mainContentCloser.CloseMainContent();
        }

        #region Operation Panel

        private void IncreaseTitleTrimCharCountAction() => TitleTrimCharCount++;

        private void DecreaseTitleTrimCharCountAction()
        {
            if (TitleTrimCharCount > 0) TitleTrimCharCount--;
        }

        private void IncreaseTitlePatternIndexAction() => TitlePatternIndex++;

        private void DecreaseTitlePatternIndexAction()
        {
            if (TitlePatternIndex > 0) TitlePatternIndex--;
        }

        private void TrimTitleStartAction()
        {
            if (SelectedModels.Any())
                foreach (var model in SelectedModels)
                    model.Title = model.Title.Length > TitleTrimCharCount
                        ? model.Title.Remove(0, TitleTrimCharCount)
                        : string.Empty;
        }

        private void TrimTitleEndAction()
        {
            if (SelectedModels.Any())
                foreach (var model in SelectedModels)
                    model.Title = model.Title.Length > TitleTrimCharCount
                        ? model.Title.Remove(model.Title.Length - TitleTrimCharCount, TitleTrimCharCount)
                        : string.Empty;
        }

        private void RemovePatternAction()
        {
            if (SelectedModels.Any())
                foreach (var model in SelectedModels)
                    model.Title = model.Title.Replace(TitlePatternText, string.Empty);
        }

        private void InsertPatternStartAction()
        {
            if (SelectedModels.Any())
                foreach (var model in SelectedModels)
                    model.Title = TitlePatternText + model.Title;
        }

        private void InsertPatternEndAction()
        {
            if (SelectedModels.Any())
                foreach (var model in SelectedModels)
                    model.Title += TitlePatternText;
        }

        private void InsertPatternIndexAction()
        {
            if (SelectedModels.Any())
            {
                foreach (var model in SelectedModels)
                {
                    if (model.Title.Length < TitlePatternIndex)
                        model.Title += TitlePatternText;
                    else
                        model.Title = model.Title.Insert(TitlePatternIndex, TitlePatternText);
                }
            }
        }

        private void RemoveTrackNumberAction()
        {
            if (SelectedModels.Any())
                foreach (var model in SelectedModels)
                    model.TrackNumber = null;
        }

        private void InsertTrackNumberAction()
        {
            if (SelectedModels.Any())
            {
                int track = 0;
                foreach (var model in SelectedModels)
                    model.TrackNumber = ++track;
            }
        }

        private void SetAlbumAction()
        {
            if (SelectedModels.Any())
                foreach (var model in SelectedModels)
                    model.SetAlbum(SelectedAlbum);
        }

        private void SetVocalGenderAction()
        {
            if (SelectedModels.Any())
                foreach (var model in SelectedModels)
                    model.SetVocalGender(SelectedVocalGender);
        }

        #endregion Operation Panel

        #endregion METHODS

        #region SAVING

        private async Task SaveActionAsync()
        {
            _logger.LogDebug("Saving musics...");

            var validationResult = ValidateModels();
            if (!validationResult.IsValid)
                return;
                

            var validationResults = await RegisterMusicsAsync();
            if (!validationResult.IsValid)
            {
                return;
            }
                

            var validItems = GetValidValidationResults(validationResults);
            var invalidItems = GetInvalidValidationResults(validationResults);

            ClearSavedModels(validItems);
            SetValidationResultFromRegisterOperationIntoModel(invalidItems);

            var isAllValid = !invalidItems.Any();
            if (isAllValid)
                _dialogService.ShowMessage(MessageType.Information, "All items are saved and moved to library.");
            else
                _dialogService.ShowMessage(MessageType.Warning, "One or more items are not saved!");
        }

        private ValidationResult ValidateModels()
        {
            var validationResult = new ValidationResult();

            if (!Models.Any())
            {
                var message = "Music list is empty.";

                validationResult.AddFailure(message);

                _logger.LogWarning(message);
                _dialogService.ShowMessage(MessageType.Warning, $"Music list is empty.");

                return validationResult;
            }

            _appService.SetTargetFullPaths(Models);

            var isAllModelsValid = Models.All(m => m.IsValid);
            if (!isAllModelsValid)
            {
                var message = $"Some musics are invalid.";

                validationResult.AddFailure(message);

                _logger.LogError(message);
                _dialogService.ShowMessage(MessageType.Error, message);

                return validationResult;
            }

            return validationResult;
        }

        private async Task<IReadOnlyDictionary<CreateMusicModel, ValidationResult>> RegisterMusicsAsync()
        {
            var results = new Dictionary<CreateMusicModel, ValidationResult>();
            var modelsCount = Models.Count;

            for (int i = 0; i < modelsCount; i++)
            {
                var model = Models[i];

                _dialogService.ShowMessage(MessageType.Status,
                    $"Registering {i + 1:00} of {modelsCount:00} items: [{model.TargetFullPath}]...");

                results.Add(model, await _appService.RegisterMusicAsync(model));
            }

            return results;
        }

        private static IReadOnlyDictionary<CreateMusicModel, ValidationResult> GetValidValidationResults(
            IReadOnlyDictionary<CreateMusicModel, ValidationResult> validationResults)
        {
            return validationResults.Where(kv => kv.Value.IsValid).ToDictionary(kv => kv.Key, kv => kv.Value);
        }

        private static IReadOnlyDictionary<CreateMusicModel, ValidationResult> GetInvalidValidationResults(
            IReadOnlyDictionary<CreateMusicModel, ValidationResult> validationResults)
        {
            return validationResults.Where(kv => !kv.Value.IsValid).ToDictionary(kv => kv.Key, kv => kv.Value);
        }

        private void ClearSavedModels(IReadOnlyDictionary<CreateMusicModel, ValidationResult> validItems)
        {
            foreach (var model in validItems.Keys)
            {
                _logger.LogInformation($"Music saved to <{model.TargetFullPath}>.");

                Models.Remove(model);
            }
        }

        private void SetValidationResultFromRegisterOperationIntoModel(
            IReadOnlyDictionary<CreateMusicModel, ValidationResult> invalidItems)
        {
            foreach (var kv in invalidItems)
            {
                var model = kv.Key;
                var result = kv.Value;

                model.ValidationResult = result;

                _logger.LogError($"Music <{model.SourceFullPath}> not saved. Errors:\n\t{result}");
            }
        }

        #endregion SAVING

        #region CTOR

        public RegisterMusicViewModel(
            ILogger logger,
            IDialogService dialogService,
            IMainContentCloser contentCloser,
            IRegisterMusicAppService appService)
            : base(logger, dialogService, contentCloser)
        {
            _appService = appService;

            SaveCommand = new AsyncRelayCommand(SaveActionAsync);
            CloseCommand = new RelayCommand(CloseAction);

            SetVocalGenderCommand = new RelayCommand(SetVocalGenderAction);
            SetAlbumCommand = new RelayCommand(SetAlbumAction);

            

            IncreaseTitleTrimCharCountCommand = new RelayCommand(IncreaseTitleTrimCharCountAction);
            DecreaseTitleTrimCharCountCommand = new RelayCommand(DecreaseTitleTrimCharCountAction);
            IncreaseTitlePatternIndexCommand = new RelayCommand(IncreaseTitlePatternIndexAction);
            DecreaseTitlePatternIndexCommand = new RelayCommand(DecreaseTitlePatternIndexAction);
            TrimTitleStartCommand = new RelayCommand(TrimTitleStartAction);
            TrimTitleEndCommand = new RelayCommand(TrimTitleEndAction);
            RemovePatternCommand = new RelayCommand(RemovePatternAction);
            InsertPatternStartCommand = new RelayCommand(InsertPatternStartAction);
            InsertPatternEndCommand = new RelayCommand(InsertPatternEndAction);
            InsertPatternIndexCommand = new RelayCommand(InsertPatternIndexAction);
            RemoveTrackNumberCommand = new RelayCommand(RemoveTrackNumberAction);
            InsertTrackNumberCommand = new RelayCommand(InsertTrackNumberAction);
        }

        #endregion CTOR
    }
}

// Truth table RegisterMusic
//                         src | des | TTV |  RESULT => (TTV = CheckTrackAndTitle.IsValid)
//Download -> Already    |  0  |  1  |  x  | .Replace
//Download -> NewMusic   |  0  |  0  |  1  | .AddNew
//Download -> TrackTitle |  0  |  0  |  0  | .ERRT
//Music    -> NewMusic   |  1  |  0  |  1  | .Change
//Music    -> TrackTitle |  1  |  0  |  0  | .ERRT
//Music    -> Already    |  1  |  1  |  x  | .ERR
//
//srcRegistered    |   desRegistered   |   TrackOrTitle    |  RESULT
//      0          |          1        |         X         |   Replace
//      1          |          1        |         X         |   Err
//      1          |          0        |         1         |   Err
//      0          |          0        |         1         |   Err
//      1          |          0        |         0         |   Change
//      0          |          0        |         0         |   AddNew