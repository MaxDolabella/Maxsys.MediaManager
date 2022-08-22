using CommunityToolkit.Mvvm.Input;
using Maxsys.MediaManager.CoreDomain.Interfaces;
using Maxsys.MediaManager.MusicContext.ApplicationMVVM.Commands;
using Maxsys.MediaManager.MusicContext.ApplicationMVVM.Interfaces.Services;
using Maxsys.MediaManager.MusicContext.ApplicationMVVM.Models;
using Maxsys.MediaManager.MusicContext.Domain.DTO;
using Maxsys.MediaManager.MusicContext.Domain.ValueObjects;
using Maxsys.ModelCore.Interfaces.Services;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Maxsys.MediaManager.MusicContext.ApplicationMVVM.ViewModels
{
    public sealed class RegisterMusicViewModel : ViewModelCollectionBase<CreateMusicModel>
    {
        #region FIELDS

        private readonly IRegisterMusicAppService _appService;

        private readonly static ReadOnlyObservableCollection<CreateMusicModel> EMPTY_COLLECTION
            = Array.Empty<CreateMusicModel>().ToReadOnlyObservableCollection();

        private ReadOnlyObservableCollection<CatalogDetailDTO> _musicCatalogs;
        private ReadOnlyObservableCollection<ArtistDetailsDTO> _artists;
        private ReadOnlyObservableCollection<AlbumInfoDTO> _albums;
        private ReadOnlyObservableCollection<SongInfoDTO> _musics;
        private ReadOnlyObservableCollection<CreateMusicModel> _selectedModels;
        private CatalogDetailDTO _selectedMusicCatalog;
        private ArtistDetailsDTO _selectedArtist;
        private AlbumInfoDTO _selectedAlbum;
        private VocalGender _selectedVocalGender;

        private int _titleTrimCharCount;
        private string _titlePatternText;
        private int _titlePatternIndex;

        #endregion FIELDS

        #region PROPS

        public IReadOnlyList<VocalGender> VocalGenders { get; }
            = new ReadOnlyCollection<VocalGender>(Enum.GetValues<VocalGender>());

        public ReadOnlyObservableCollection<CatalogDetailDTO> MusicCatalogs
        {
            get => _musicCatalogs;
            private set => SetProperty(ref _musicCatalogs, value);
        }

        public ReadOnlyObservableCollection<ArtistDetailsDTO> DisplayableArtists
            => _artists
            ?.Where(a => a.MusicCatalogId == SelectedMusicCatalog.MusicCatalogId)
            ?.OrderBy(a => a.ArtistName)
            ?.ToReadOnlyObservableCollection();

        public ReadOnlyObservableCollection<AlbumInfoDTO> DisplayableAlbums
            => _albums
            ?.Where(a => a.ArtistId == SelectedArtist?.ArtistId)
            ?.OrderBy(a => a.AlbumDirectory)
            ?.ToReadOnlyObservableCollection();

        public ReadOnlyObservableCollection<SongInfoDTO> DisplayableMusics
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

        public CatalogDetailDTO SelectedMusicCatalog
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

        public ArtistDetailsDTO SelectedArtist
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

        private char _charToReplace;

        public char CharToReplace
        {
            get => _charToReplace;
            set => SetProperty(ref _charToReplace, value);
        }

        private char _charToBeReplaced;

        public char CharToBeReplaced
        {
            get => _charToBeReplaced;
            set => SetProperty(ref _charToBeReplaced, value);
        }

        #endregion PROPS

        #region COMMANDS

        public ICommand SaveCommand { get; }

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
        public ICommand ReplaceCharCommand { get; }
        public ICommand CapitalizeToTitleCaseCommand { get; }

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

        public async Task LoadMainComboBoxesActionAsync()
        {
            _logger.LogDebug("Loading comboBoxes...");

            // Get lists
            MusicCatalogs = (await _appService.GetMusicCatalogsAsync()).ToReadOnlyObservableCollection();
            _artists = (await _appService.GetArtistsAsync()).ToReadOnlyObservableCollection();
            _albums = (await _appService.GetAlbumsAsync()).ToReadOnlyObservableCollection();
            _musics = (await _appService.GetMusicsAsync()).ToReadOnlyObservableCollection();

            _logger.LogDebug("ComboBoxes loaded.");
        }

        public async Task ReloadMusicsAsync()
        {
            _musics = (await _appService.GetMusicsAsync()).ToReadOnlyObservableCollection();

            if (SelectedAlbum is not null)
                OnPropertyChanged(nameof(DisplayableMusics));
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

        private void ReplaceCharAction()
        {
            if (!(char.IsWhiteSpace(CharToReplace) || char.IsWhiteSpace(CharToBeReplaced)))
            {
                foreach (var model in SelectedModels)
                    model.Title = model.Title.Replace(CharToBeReplaced, CharToReplace);
            }
        }

        private void CapitalizeToTitleCaseAction()
        {
            var textInfo = new CultureInfo("en-US", false).TextInfo;

            foreach (var model in SelectedModels)
                model.Title = textInfo.ToTitleCase(model.Title);
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

        #region CTOR

        public RegisterMusicViewModel(
            ILogger logger,
            IDialogService dialogService,
            IMainContentCloser contentCloser,
            IRegisterMusicAppService appService)
            : base(logger, dialogService, contentCloser)
        {
            _appService = appService;

            SaveCommand = new RegisterMusicCommand(this, logger, appService, dialogService);

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
            ReplaceCharCommand = new RelayCommand(ReplaceCharAction);
            CapitalizeToTitleCaseCommand = new RelayCommand(CapitalizeToTitleCaseAction);
        }

        #endregion CTOR
    }
}

// Truth table RegisterMusic
//                         src | des | TTV |  RESULT => (TTV = CheckTrackAndTitle.IsValid)
//Download -> Already    |  0  |  1  |  x  | .Replace
//Download -> NewMusic   |  0  |  0  |  1  | .AddNew
//Download -> TrackTitle |  0  |  0  |  0  | .ERRT
//Song    -> NewMusic   |  1  |  0  |  1  | .Change
//Song    -> TrackTitle |  1  |  0  |  0  | .ERRT
//Song    -> Already    |  1  |  1  |  x  | .ERR
//
//srcRegistered    |   desRegistered   |   TrackOrTitle    |  RESULT
//      0          |          1        |         X         |   Replace
//      1          |          1        |         X         |   Err
//      1          |          0        |         1         |   Err
//      0          |          0        |         1         |   Err
//      1          |          0        |         0         |   Change
//      0          |          0        |         0         |   AddNew