using Maxsys.MediaManager.CoreDomain.Commands;
using Maxsys.MediaManager.CoreDomain.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;

namespace Maxsys.MediaManager.MusicContext.WPF.Commands.MainWindow;

public class OpenViewCommand : CommandBase
{
    private readonly ILogger _logger;
    private readonly IServiceProvider _serviceProvider;
    private readonly IMainContentOwner _mainContentOwner;

    
    public OpenViewCommand(ILogger logger, IMainContentOwner mainContentOwner, IServiceProvider serviceProvider)
    {
        _logger = logger;
        _mainContentOwner = mainContentOwner;
        _serviceProvider = serviceProvider;
    }

    public override bool CanExecute(object parameter)
    {
        return parameter is not null
            && parameter is Type type
            && typeof(IView).IsAssignableFrom(type)
            && base.CanExecute(parameter);
    }

    public override void Execute(object parameter)
    {
        var viewType = parameter as Type;
        try
        {
            _logger.LogDebug($"OpenViewAction() called...");

            _mainContentOwner.CloseMainContent();

            var view = (IView)_serviceProvider.GetRequiredService(viewType);

            _mainContentOwner.SetMainContent(view);

            _logger.LogDebug($"{viewType.Name} in the content container.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error at OpenViewAction()");
        }
    }
}