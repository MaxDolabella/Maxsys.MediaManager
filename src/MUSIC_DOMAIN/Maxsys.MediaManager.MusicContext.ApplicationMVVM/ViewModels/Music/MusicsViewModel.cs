using CommunityToolkit.Mvvm.Input;
using Maxsys.MediaManager.CoreDomain.Interfaces;
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
    public sealed class MusicsViewModel : ViewModelBase
    {
        #region FIELDS

        private readonly IMusicListAppService _appService;

        private string _searchTerm;
        private ReadOnlyObservableCollection<MusicListModel> _models;
        private ReadOnlyObservableCollection<MusicListModel> _displayedMusics;
        private MusicListModel _selectedMusic;

        #endregion FIELDS

        #region PROPS

        public ReadOnlyObservableCollection<MusicListModel> Models
        {
            get => _models;
            set => SetProperty(ref _models, value);
        }

        public ReadOnlyObservableCollection<MusicListModel> DisplayedMusics
        {
            get => _displayedMusics;
            set => SetProperty(ref _displayedMusics, value);
        }

        public MusicListModel SelectedMusic
        {
            get => _selectedMusic;
            set => SetProperty(ref _selectedMusic, value);
        }

        public string SearchTerm
        {
            get => _searchTerm;
            set => SetProperty(ref _searchTerm, value);
        }

        #endregion PROPS

        #region COMMANDS

        public ICommand SearchCommand { get; }
        public ICommand DeleteMusicCommand { get; }
        public ICommand CloseCommand { get; }

        #endregion COMMANDS

        #region METHODS

        public override async Task ViewLoadedAsync()
        {
            await LoadMusics();
        }

        private async Task LoadMusics()
        {
            _logger.LogDebug("Loading musics...");

            DisplayedMusics = Models = (await _appService.GetMusicsAsync()).ToReadOnlyObservableCollection();

            _logger.LogDebug("Musics loaded.");
        }

        private async Task SearchActionAsync()
        {
            await Task.Run(() => SearchAction());
        }

        private void SearchAction()
        {
            _logger.LogDebug("Searching musics...");

            if (string.IsNullOrWhiteSpace(SearchTerm))
            {
                DisplayedMusics = Models;
            }
            else
            {
                var searchTerm = SearchTerm.ToUpper();

                DisplayedMusics = Models.Where(m
                    => m.MusicTitle.ToUpper().Contains(searchTerm)
                    || m.MusicCatalogName.ToUpper().Contains(searchTerm)
                    || m.ArtistName.ToUpper().Contains(searchTerm)
                    || m.AlbumName.ToUpper().Contains(searchTerm)
                    || m.MusicCoveredArtist.ToUpper().Contains(searchTerm)
                    || m.MusicFeaturedArtist.ToUpper().Contains(searchTerm))
                    .OrderBy(m => m.MusicFullPath)
                    .ToReadOnlyObservableCollection();
            }
        }

        private void CloseAction()
        {
            _mainContentCloser.CloseMainContent();
        }

        #endregion METHODS

        #region CTOR

        public MusicsViewModel(
            ILogger logger,
            IDialogService dialogService,
            IQuestionDialogService questionDialogService,
            IMainContentCloser contentCloser,
            IMusicListAppService appService)
            : base(logger, dialogService, contentCloser)
        {
            _appService = appService;

            DeleteMusicCommand = new DeleteMusicCommand(this, logger, questionDialogService, dialogService, appService);
            SearchCommand = new AsyncRelayCommand(SearchActionAsync);
            CloseCommand = new RelayCommand(CloseAction);
        }

        #endregion CTOR
    }
}