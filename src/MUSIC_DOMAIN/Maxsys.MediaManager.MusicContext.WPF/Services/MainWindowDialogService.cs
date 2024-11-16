using System.Windows;
using Maxsys.Core.Services;

namespace Maxsys.MediaManager.MusicContext.WPF.Services
{
    public class MainWindowDialogService : ServiceBase, IDialogService
    {
        public MainWindowDialogService(MainWindow window)
        {
            Owner = window;
        }

        // TODO Make Owner readonly
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
    }
}