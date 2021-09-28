using Maxsys.ModelCore.Interfaces.Services;
using System;
using System.Windows;

namespace Maxsys.MediaManager.MusicContext.WPF.Services
{
    public class MainWindowDialogService : IDialogService
    {
        public MainWindowDialogService(MainWindow window)
        {
            Owner = window;
        }

        public object Owner { get; set; }
        private MainWindow Window => (MainWindow)Owner;//Owner is not DependencyObject dObj ? null : Window.GetWindow(dObj);

        public void ShowMessage(MessageType messageType, string message, string title = null)
        {
            var messageBoxImage = messageType switch
            {
                MessageType.Information => MessageBoxImage.Information,
                MessageType.Warning => MessageBoxImage.Warning,
                MessageType.Error => MessageBoxImage.Error,
                //MessageType.Status => MessageBoxImage.None,
                _ => MessageBoxImage.None,
            };

            _ = MessageBox.Show(Window, message, title, MessageBoxButton.OK, messageBoxImage);
        }

        #region DIPOSABLE IMPLEMENTATION

        public void Dispose() => GC.SuppressFinalize(this);

        #endregion DIPOSABLE IMPLEMENTATION
    }
}