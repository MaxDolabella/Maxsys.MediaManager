using CommunityToolkit.Mvvm.ComponentModel;
using Maxsys.MediaManager.CoreDomain.Interfaces;
using Maxsys.ModelCore.Interfaces.Services;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace Maxsys.MediaManager.MusicContext.ApplicationMVVM.ViewModels
{
    public abstract class ViewModelBase : ObservableObject
    {
        protected readonly ILogger _logger;
        protected readonly IDialogService _dialogService;
        protected readonly IMainContentCloser _mainContentCloser;

        public abstract Task ViewLoadedAsync();

        protected ViewModelBase(ILogger logger, IDialogService dialogService, IMainContentCloser mainContentCloser)
        {
            _logger = logger;
            _dialogService = dialogService;
            _mainContentCloser = mainContentCloser;
        }
    }
}