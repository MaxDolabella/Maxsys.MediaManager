//using Maxsys.MediaManager.CoreDomain.Commands;
//using Maxsys.MediaManager.CoreDomain.Interfaces;
//using Maxsys.MediaManager.MusicContext.ApplicationMVVM.Interfaces;
//using Microsoft.Extensions.DependencyInjection;
//using Microsoft.Extensions.Logging;
//using System;
//using System.Windows.Input;

//namespace Maxsys.MediaManager.MusicContext.ApplicationMVVM.Commands
//{
//    public class OpenViewCommand : CommandBase
//    {
//        //private readonly Action<Type> _action;
//        private readonly ILogger _logger;
//        private readonly IServiceProvider _serviceProvider;
//        private readonly IMainContentOwner _mainContentOwner;

//        public OpenViewCommand(Action<Type> action)
//        {
//            //_action = action;
//        }

//        public OpenViewCommand(IMainContentOwner mainContentOwner, ILogger logger, IServiceProvider serviceProvider)
//        {
//            _mainContentOwner = mainContentOwner;
//            _logger = logger;
//            _serviceProvider = serviceProvider;
//        }

//        public override bool CanExecute(object parameter)
//        {
//            return parameter is not null
//                && parameter is Type type
//                && typeof(IView).IsAssignableFrom(type)
//                && base.CanExecute(parameter);
//        }

//        public override void Execute(object parameter)
//        {
//            //_action?.Invoke(parameter as Type);

//            var viewType = parameter as Type;
//            try
//            {
//                _logger.LogDebug($"OpenViewAction() called...");

//                _mainContentOwner.CloseMainContent();

//                var view = (IView)_serviceProvider.GetRequiredService(viewType);

//                _mainContentOwner.SetMainContent(view);

//                _logger.LogDebug($"{viewType.Name} in the content container.");
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError(ex, "Error at OpenViewAction()");
//            }
//        }
//    }
//}