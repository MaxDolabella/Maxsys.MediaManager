using Maxsys.MediaManager.CoreDomain.Interfaces;
using Maxsys.MediaManager.MusicContext.ApplicationMVVM.Models;
using Maxsys.ModelCore.Interfaces.Services;
using Microsoft.Extensions.Logging;
using System.Collections.ObjectModel;

namespace Maxsys.MediaManager.MusicContext.ApplicationMVVM.ViewModels
{
    public abstract class ViewModelCollectionBase<TModel> : ViewModelBase
        //where TModel : ObservableObject, new()
        where TModel : ValidableModelBase, new()
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