using System;
using CommunityToolkit.Mvvm.ComponentModel;

namespace Maxsys.MediaManager.MusicContext.WPF.ViewModels;

/// <remarks>
/// <see cref="IDisposable"/>: <br/>
/// Do not change <see cref="Dispose()">Dispose()</see> code;<br/>
/// Put cleanup code in <see cref="Dispose(bool)">Dispose(disposing)</see> method.
/// </remarks>
public abstract class ViewModelBase : ObservableObject, IDisposable
{
    private bool disposedValue;

    protected virtual void Dispose(bool disposing)
    {
        if (!disposedValue)
        {
            if (disposing)
            {
                Disposing();
            }

            disposedValue = true;
        }
    }

    public void Dispose()
    {
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Disposing()
    { }
}