using System;
using System.Windows;
using Maxsys.MediaManager.CoreDomain.Interfaces;

namespace Maxsys.MediaManager.MusicContext.WPF.Services
{
    public class GlobalQuestionDialogService : IQuestionDialogService
    {
        public object Owner { get; }
        private MainWindow Window => (MainWindow)Owner;

        public GlobalQuestionDialogService(MainWindow window)
        {
            Owner = window;
        }

        public IQuestionDialogService.QuestionDialogResult ShowQuestion(string question, string title = null)
        {
            var messageBoxResult = MessageBox.Show(Window, question, title, MessageBoxButton.YesNo, MessageBoxImage.Question);

            return (IQuestionDialogService.QuestionDialogResult)messageBoxResult;
        }

        #region DIPOSABLE IMPLEMENTATION

        public void Dispose() => GC.SuppressFinalize(this);

        #endregion DIPOSABLE IMPLEMENTATION
    }
}