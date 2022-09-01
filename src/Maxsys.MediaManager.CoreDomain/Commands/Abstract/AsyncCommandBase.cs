using Maxsys.MediaManager.CoreDomain.Interfaces;
using System;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Maxsys.MediaManager.CoreDomain.Commands;

/// <summary>
/// Implementation of Asynchronous Command with "Fire-And-Forget-Safe-Async" approach.
/// <br/>
/// See <see href="https://johnthiriet.com/mvvm-going-async-with-async-command/">"MVVM - Going async with async command"
/// article</see>.
/// </summary>
public abstract class AsyncCommandBase : IAsyncCommand
{
    public event EventHandler? CanExecuteChanged;

    private bool _isExecuting;
    private readonly IErrorHandler? _errorHandler;

    public AsyncCommandBase(IErrorHandler? errorHandler = null)
    {
        _errorHandler = errorHandler;
    }

    public abstract bool CanExecute();

    public abstract Task ExecuteAsync();

    public void RaiseCanExecuteChanged()
    {
        CanExecuteChanged?.Invoke(this, EventArgs.Empty);
    }

    #region Explicit implementations

    bool ICommand.CanExecute(object? parameter)
    {
        return !_isExecuting && CanExecute();
    }

    void ICommand.Execute(object? parameter)
    {
        //ExecuteAsync().FireAndForgetSafeAsync(_errorHandler);
        Task.Run(async () =>
        {
            if (CanExecute())
            {
                try
                {
                    _isExecuting = true;
                    await ExecuteAsync();
                }
                finally
                {
                    _isExecuting = false;
                }
            }

            RaiseCanExecuteChanged();
        }).FireAndForgetSafeAsync(_errorHandler);
    }

    #endregion Explicit implementations
}