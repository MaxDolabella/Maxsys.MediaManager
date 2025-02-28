using System;
using CommunityToolkit.Mvvm.Input;

namespace Maxsys.MediaManager.MusicContext.WPF.Commands.Abstrations;

public abstract class CommandBase : IRelayCommand
{
    public event EventHandler? CanExecuteChanged;

    public void NotifyCanExecuteChanged()
    {
        CanExecuteChanged?.Invoke(this, EventArgs.Empty);
    }

    public virtual bool CanExecute(object? parameter) => true;

    public abstract void Execute(object? parameter);
}