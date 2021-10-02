using CommunityToolkit.Mvvm.Input;
using Maxsys.MediaManager.CoreDomain.Interfaces;
using Maxsys.MediaManager.MusicContext.ApplicationMVVM.Commands;
using Maxsys.ModelCore.Interfaces.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Maxsys.MediaManager.MusicContext.ApplicationMVVM.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IMainContentOwner _mainContentOwner;
        private readonly IAppCloser _appCloser;

        private static readonly string s_appVersion = Assembly.GetEntryAssembly()
            .GetCustomAttribute<AssemblyInformationalVersionAttribute>().InformationalVersion;

        public string AppVersion => s_appVersion;

        #region PROPERTIES

        private string _currentMessage;

        public string CurrentMessage
        {
            get => _currentMessage;
            set => SetProperty(ref _currentMessage, value);
        }

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

        public ICommand OpenViewCommand { get; set; }
        public ICommand CloseAppCommand { get; set; }

        #endregion COMMANDS

        #region METHODS

        public override async Task ViewLoadedAsync()
        {
            await Task.Run(() =>
            {
                _logger.LogDebug("MainWindow Loaded.");

                CurrentMessage = "READY.";
                CurrentMessageType = MessageType.Status;
            });
        }

        private void OpenViewAction(Type viewType)
        {
            try
            {
                _logger.LogDebug($"OpenViewAction() called...");

                _mainContentOwner.CloseMainContent();

                var view = (IView)_serviceProvider.GetRequiredService(viewType);

                _mainContentOwner.SetMainContent(view);

                _logger.LogDebug($"{viewType.Name} in the content container.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error at OpenViewAction()");
            }
        }

        private void CloseAppAction() => _appCloser.CloseApp();

        #endregion METHODS

        #region CTOR

        public MainWindowViewModel(
            ILogger logger,
            IHost host,
            IMainContentOwner mainContentOwner,
            IAppCloser appCloser)
            : base(logger, null, mainContentOwner)
        {
            _serviceProvider = host.Services.CreateScope().ServiceProvider;
            _mainContentOwner = mainContentOwner;
            _appCloser = appCloser;

            OpenViewCommand = new OpenViewCommand(OpenViewAction);
            CloseAppCommand = new RelayCommand(CloseAppAction);
        }

        #endregion CTOR
    }
}