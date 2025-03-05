using System.Windows.Input;

namespace Maxsys.MediaManager.CoreDomain.Commands;

/// <summary>
/// Provides an interface to an Asynchronous Command.
/// <br/>
/// See <see href="https://johnthiriet.com/mvvm-going-async-with-async-command/">"MVVM - Going async with async command"
/// article</see>.
/// </summary>
public interface IAsyncCommand : IAsyncCommand<object>
{ }

/// <summary>
/// Provides an interface to an Asynchronous Command.
/// <br/>
/// See <see href="https://johnthiriet.com/mvvm-going-async-with-async-command/">"MVVM - Going async with async command"
/// article</see>.
/// </summary>
public interface IAsyncCommand<TParam> : ICommand
{
    Task ExecuteAsync(TParam? parameter);

    bool CanExecute(TParam? parameter);
}