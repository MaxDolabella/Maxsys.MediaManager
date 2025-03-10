﻿using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Maxsys.MediaManager.CoreDomain.Interfaces;

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

    public abstract bool CanExecute(object? parameter = null);

    public abstract Task ExecuteAsync(object? parameter = null);

    public void RaiseCanExecuteChanged()
    {
        CanExecuteChanged?.Invoke(this, EventArgs.Empty);
    }

    #region Explicit implementations

    bool ICommand.CanExecute(object? parameter)
    {
        return !_isExecuting && CanExecute(parameter);
    }

    void ICommand.Execute(object? parameter)
    {
        //ExecuteAsync().FireAndForgetSafeAsync(_errorHandler);
        Task.Run(async () =>
        {
            if (CanExecute(parameter))
            {
                try
                {
                    _isExecuting = true;
                    await ExecuteAsync(parameter);
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


/// <summary>
/// Implementation of Asynchronous Command with "Fire-And-Forget-Safe-Async" approach.
/// <br/>
/// See <see href="https://johnthiriet.com/mvvm-going-async-with-async-command/">"MVVM - Going async with async command"
/// article</see>.
/// </summary>
public class AsyncCommand<T> : IAsyncCommand<T>
{
    public event EventHandler? CanExecuteChanged;

    private bool _isExecuting;
    private readonly Func<T, Task> _execute;
    private readonly Func<T, bool>? _canExecute;
    private readonly IErrorHandler? _errorHandler;

    public AsyncCommand(Func<T, Task> execute, Func<T, bool>? canExecute = null, IErrorHandler? errorHandler = null)
    {
        _execute = execute;
        _canExecute = canExecute;
        _errorHandler = errorHandler;
    }

    public bool CanExecute(T? parameter)
    {
        return parameter is null || !_isExecuting && (_canExecute?.Invoke(parameter) ?? true);
    }

    public async Task ExecuteAsync(T? parameter)
    {
        if (CanExecute(parameter))
        {
            try
            {
                _isExecuting = true;
                await _execute(parameter!);
            }
            finally
            {
                _isExecuting = false;
            }
        }

        RaiseCanExecuteChanged();
    }

    public void RaiseCanExecuteChanged()
    {
        CanExecuteChanged?.Invoke(this, EventArgs.Empty);
    }

    #region Explicit implementations

    bool ICommand.CanExecute(object? parameter)
    {
        return parameter is null || CanExecute((T)parameter);
    }

    void ICommand.Execute(object? parameter)
    {
        ExecuteAsync((T?)parameter).FireAndForgetSafeAsync(_errorHandler);
    }

    #endregion Explicit implementations
}