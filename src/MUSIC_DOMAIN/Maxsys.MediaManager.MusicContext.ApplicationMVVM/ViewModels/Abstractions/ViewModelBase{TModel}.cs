//using Maxsys.MediaManager.CoreDomain.Interfaces;
//using Maxsys.MediaManager.MusicContext.ApplicationMVVM.ViewModels.Abstractions;
//using Maxsys.ModelCore.Interfaces.Services;
//using Microsoft.Extensions.Logging;

//namespace Maxsys.MediaManager.MusicContext.ApplicationMVVM.ViewModels
//{
//    public abstract class ViewModelBase<TModel> : ViewModelBase
//        //where TModel : ObservableObject, new()
//        where TModel : ValidableViewModelBase, new()
//    {
//        protected TModel _model;

//        public TModel Model
//        {
//            get => _model;
//            set => SetProperty(ref _model, value);
//        }

//        protected ViewModelBase(ILogger logger, IDialogService dialogService, IMainContentCloser contentCloser)
//            : base(logger, dialogService, contentCloser)
//        {
//            Model = new();
//        }
//    }
//}