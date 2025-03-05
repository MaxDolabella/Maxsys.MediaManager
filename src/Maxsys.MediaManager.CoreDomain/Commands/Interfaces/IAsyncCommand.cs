using System.Threading.Tasks;
using System.Windows.Input;

namespace Maxsys.MediaManager.CoreDomain.Commands;

/// <summary>
/// Provides an interface to an Asynchronous Command.
/// <br/>
/// See <see href="https://johnthiriet.com/mvvm-going-async-with-async-command/">"MVVM - Going async with async command"
/// article</see>.
/// </summary>
public interface IAsyncCommand : ICommand
{
    Task ExecuteAsync();

    bool CanExecute();
}

/// <summary>
/// Provides an interface to an Asynchronous Command.
/// <br/>
/// See <see href="https://johnthiriet.com/mvvm-going-async-with-async-command/">"MVVM - Going async with async command"
/// article</see>.
/// </summary>
public interface IAsyncCommand<T> : ICommand
{
    Task ExecuteAsync(T? parameter);

    bool CanExecute(T? parameter);
}