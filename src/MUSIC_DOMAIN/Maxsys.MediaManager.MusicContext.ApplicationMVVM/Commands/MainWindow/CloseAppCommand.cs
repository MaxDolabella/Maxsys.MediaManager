﻿//using Maxsys.MediaManager.CoreDomain.Commands;
//using Maxsys.MediaManager.CoreDomain.Interfaces;
//using Microsoft.Extensions.Logging;

//namespace Maxsys.MediaManager.MusicContext.ApplicationMVVM.Commands;

//public class CloseAppCommand : CommandBase
//{
//    private readonly ILogger _logger;
//    private readonly IAppCloser _appCloser;

//    public CloseAppCommand(ILogger<CloseAppCommand> logger, IAppCloser appCloser)
//    {
//        _logger = logger;
//        _appCloser = appCloser;
//    }

//    public override void Execute(object? parameter)
//    {
//        _logger.LogInformation("Closing the application");

//        _appCloser.CloseApp();
//    }
//}