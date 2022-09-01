using Maxsys.MediaManager.CoreDomain.Interfaces;

namespace System.Threading.Tasks;

/// <summary>
/// Provides a secure execution of an <see langword="async"/> <see langword="void"/> method.
/// <br/>
/// See 'Removing async void' <see href="https://johnthiriet.com/removing-async-void/">article</see>
/// </summary>
public static class TaskExtensions
{
    public static async void FireAndForgetSafeAsync(this Task task, IErrorHandler? handler = null)
    {
        try
        {
            await task;
        }
        catch (Exception ex)
        {
            handler?.HandleError(ex);
        }
    }
}