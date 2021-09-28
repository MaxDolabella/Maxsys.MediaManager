using Maxsys.MediaManager.MusicContext.ApplicationMVVM.Interfaces;
using System;
using System.Windows.Input;

namespace Maxsys.MediaManager.MusicContext.ApplicationMVVM.Commands
{
    public class OpenViewCommand : ICommand
    {
        private readonly Action<Type> _action;

        public OpenViewCommand(Action<Type> action)
        {
            _action = action;
        }

        public bool CanExecute(object parameter)
        {
            return parameter is not null
                && parameter is Type type
                && typeof(IView).IsAssignableFrom(type);
        }

        public void Execute(object parameter)
        {
            _action?.Invoke(parameter as Type);
        }

        public event EventHandler CanExecuteChanged;
    }
}