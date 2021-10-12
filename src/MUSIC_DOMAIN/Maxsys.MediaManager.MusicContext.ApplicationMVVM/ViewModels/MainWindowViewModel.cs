using Maxsys.MediaManager.CoreDomain.Interfaces;
using Maxsys.MediaManager.MusicContext.ApplicationMVVM.Commands;
using Maxsys.MediaManager.MusicContext.ApplicationMVVM.Store;
using Maxsys.ModelCore.Interfaces.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Maxsys.MediaManager.MusicContext.ApplicationMVVM.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        private static readonly string s_appVersion = Assembly.GetEntryAssembly()
            .GetCustomAttribute<AssemblyInformationalVersionAttribute>().InformationalVersion;

        private readonly NavigationStore _navigationStore;

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

        #endregion METHODS

        #region CTOR

        public MainWindowViewModel(
            ILogger logger,
            IHost host,
            IMainContentOwner mainContentOwner,
            IAppCloser appCloser)
            : base(logger, null, mainContentOwner)
        {
            var serviceProvider = host.Services.CreateScope().ServiceProvider;

            OpenViewCommand = new OpenViewCommand(mainContentOwner, logger, serviceProvider);
            CloseAppCommand = new CloseAppCommand(logger, appCloser);

            _navigationStore = serviceProvider.GetRequiredService<NavigationStore>();
        }

        #endregion CTOR
    }
}