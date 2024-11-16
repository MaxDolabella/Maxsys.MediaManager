using System;
using System.Reflection;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using Maxsys.MediaManager.CoreDomain.Interfaces;
using Maxsys.MediaManager.MusicContext.ApplicationMVVM.Store;
using Maxsys.MediaManager.MusicContext.WPF.Commands.MainWindow;
using Maxsys.MediaManager.MusicContext.WPF.Services;
using Microsoft.Extensions.Logging;

namespace Maxsys.MediaManager.MusicContext.ApplicationMVVM.ViewModels;

public class MainWindowViewModel : ObservableObject // ViewModelBase
{
    #region FIELDS

    private static readonly string s_appVersion = Assembly
        .GetEntryAssembly()?
        .GetCustomAttribute<AssemblyInformationalVersionAttribute>()?
        .InformationalVersion ?? string.Empty;

    protected readonly IDialogService _dialogService;
    private readonly NavigationStore _navigationStore;

    #endregion FIELDS

    #region PROPERTIES

    public IView CurrentView
    {
        get => _navigationStore.CurrentView;
        set
        {
            _navigationStore.CurrentView = value;
            OnPropertyChanged();
        }
    }

    private string _currentMessage;

    public string CurrentMessage
    {
        get => _currentMessage;
        set => SetProperty(ref _currentMessage, value);
    }

    public string AppVersion => s_appVersion;

    private MessageType _currentMessageType;

    public MessageType CurrentMessageType
    {
        get => _currentMessageType;
        set
        {
            if (SetProperty(ref _currentMessageType, value))
            {
                OnPropertyChanged(nameof(IsStatusMessage));
                OnPropertyChanged(nameof(IsInfoMessage));
                OnPropertyChanged(nameof(IsWarningMessage));
                OnPropertyChanged(nameof(IsErrorMessage));
            }
        }
    }

    public bool IsStatusMessage => CurrentMessageType == MessageType.Status;
    public bool IsInfoMessage => CurrentMessageType == MessageType.Information;
    public bool IsWarningMessage => CurrentMessageType == MessageType.Warning;
    public bool IsErrorMessage => CurrentMessageType == MessageType.Error;

    #endregion PROPERTIES

    #region COMMANDS

    public ICommand OpenViewCommand { get; }
    public ICommand CloseAppCommand { get; }
    public ICommand CloseMainContentCommand { get; }

    #endregion COMMANDS

    #region METHODS

    //public override async Task LoadedCatalogsAsync()
    //{
    //    await Task.Run(() =>
    //    {
    //        _logger.LogDebug("MainWindow Loaded.");

    //        CurrentMessage = "READY.";
    //        CurrentMessageType = MessageType.Status;
    //    });
    //}

    #endregion METHODS

    #region CTOR

    public MainWindowViewModel(
        ILogger<MainWindowViewModel> logger,
        NavigationStore navigationStore,
        IMainContentOwner mainContentOwner,
        IAppCloser appCloser,
        IServiceProvider serviceProvider)
    {
        OpenViewCommand = new OpenViewCommand(logger, mainContentOwner, serviceProvider);
        CloseAppCommand = new CloseAppCommand(logger, appCloser);

        _navigationStore = navigationStore;//serviceProvider.GetRequiredService<NavigationStore>();
    }

    #endregion CTOR
}