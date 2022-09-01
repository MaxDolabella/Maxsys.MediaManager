using System;

namespace Maxsys.MediaManager.CoreDomain.Interfaces;

/// <summary>
/// Provides a handler for errors.
/// <br/>
/// <see href="https://github.com/johnthiriet/AsyncVoid/blob/af772360567db5d4869f1af69aafc86f0ea83787/AsyncVoid/AsyncVoid/IErrorHandler.cs">
/// IErrorHandler in github
/// </see>
/// </summary>
public interface IErrorHandler
{
    void HandleError(Exception ex);
}