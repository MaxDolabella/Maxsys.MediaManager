using CommunityToolkit.Mvvm.ComponentModel;
using Maxsys.MediaManager.CoreDomain.Interfaces;
using Maxsys.MediaManager.MusicContext.ApplicationMVVM.Commands;
using Maxsys.ModelCore.Interfaces.Services;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Maxsys.MediaManager.MusicContext.ApplicationMVVM.ViewModels
{
    public abstract class ViewModelBase : ObservableObject
    {
        protected readonly ILogger _logger;
        protected readonly IDialogService _dialogService;

        public ICommand CloseMainContentCommand { get; }

        public abstract Task LoadedCatalogsAsync();

        protected ViewModelBase(ILogger logger, IDialogService dialogService, IMainContentCloser mainContentCloser)
        {
            _logger = logger;
            _dialogService = dialogService;

            CloseMainContentCommand = new CloseMainContentCommand(mainContentCloser);
        }
    }
}