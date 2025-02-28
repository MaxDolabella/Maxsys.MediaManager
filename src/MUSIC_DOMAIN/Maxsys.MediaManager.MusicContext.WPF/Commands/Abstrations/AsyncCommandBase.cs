using System;
using System.ComponentModel;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.Input;

namespace Maxsys.MediaManager.MusicContext.WPF.Commands.Abstrations;

public abstract class AsyncCommandBase : IAsyncRelayCommand
{
    public event EventHandler? CanExecuteChanged;
    public event PropertyChangedEventHandler? PropertyChanged;

    #region NotImplemented

    public Task? ExecutionTask => throw new NotImplementedException();
    public bool CanBeCanceled => throw new NotImplementedException();
    public bool IsCancellationRequested => throw new NotImplementedException();
    public bool IsRunning => throw new NotImplementedException();

    public void Execute(object? parameter) => throw new NotImplementedException();

    public void Cancel() => throw new NotImplementedException();

    #endregion NotImplemented

    public abstract Task ExecuteAsync(object? parameter);

    public void NotifyCanExecuteChanged() => CanExecuteChanged?.Invoke(this, EventArgs.Empty);

    public virtual bool CanExecute(object? parameter) => true;
}