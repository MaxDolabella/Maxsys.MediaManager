using Maxsys.MediaManager.MusicContext.ApplicationMVVM.ViewModels;
using Maxsys.ModelCore.Interfaces.Services;
using System;
using System.Windows.Threading;

namespace Maxsys.MediaManager.MusicContext.WPF.Services
{
    public class MainWindowToolbarDialogService : IDialogService
    {
        #region Fields & Consts

        private const string DEFAULT_TEXT = "READY!";
        private const MessageType DEFAULT_MESSAGETYPE = MessageType.Status;
        private const int DURATION_IN_SECONDS = 5;

        private readonly DispatcherTimer _timer;

        #endregion Fields & Consts

        #region Methods

        private void DispatcherTimer_Tick(object sender, EventArgs e)
        {
            _timer.Stop();

            ShowMessage(DEFAULT_MESSAGETYPE, DEFAULT_TEXT);
        }

        #endregion Methods

        #region Ctor

        public MainWindowToolbarDialogService(MainWindow view)
        {
            Owner = view.DataContext;

            _timer = new DispatcherTimer(
                new TimeSpan(0, 0, DURATION_IN_SECONDS),
                DispatcherPriority.Normal,
                DispatcherTimer_Tick,
                App.Current.Dispatcher);
        }

        #endregion Ctor

        #region IDialogService

        public object Owner { get; set; }
        private MainWindowViewModel ViewModel => (MainWindowViewModel)Owner;

        public void ShowMessage(MessageType messageType, string message, string title = "")
        {
            ViewModel.CurrentMessageType = messageType;
            ViewModel.CurrentMessage = message;

            if (_timer.IsEnabled) _timer.Stop();

            _timer.Start();
        }

        #endregion IDialogService

        #region DIPOSABLE IMPLEMENTATION

        public void Dispose()
        {
            _timer.Tick -= DispatcherTimer_Tick;

            GC.SuppressFinalize(this);
        }

        #endregion DIPOSABLE IMPLEMENTATION
    }
}