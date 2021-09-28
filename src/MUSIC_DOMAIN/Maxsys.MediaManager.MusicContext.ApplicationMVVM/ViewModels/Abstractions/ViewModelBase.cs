using CommunityToolkit.Mvvm.ComponentModel;
using Maxsys.MediaManager.CoreDomain.Interfaces;
using Maxsys.ModelCore.Interfaces.Services;
using Microsoft.Extensions.Logging;
using System.Collections.ObjectModel;
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

    public abstract class ViewModelBase<TModel> : ViewModelBase
        where TModel : ObservableObject, new()
    {
        protected TModel _model;

        public TModel Model
        {
            get => _model;
            protected set => SetProperty(ref _model, value);
        }

        protected ViewModelBase(ILogger logger, IDialogService dialogService, IMainContentCloser contentCloser)
            : base(logger, dialogService, contentCloser)
        {
            Model = new();
        }
    }

    public abstract class ViewModelCollectionBase<TModel> : ViewModelBase
        where TModel : ObservableObject, new()
    {
        protected ObservableCollection<TModel> _models;

        public ObservableCollection<TModel> Models
        {
            get => _models;
            protected set => SetProperty(ref _models, value);
        }

        protected ViewModelCollectionBase(ILogger logger, IDialogService dialogService, IMainContentCloser contentCloser)
            : base(logger, dialogService, contentCloser)
        {
            Models = new();
        }
    }
}