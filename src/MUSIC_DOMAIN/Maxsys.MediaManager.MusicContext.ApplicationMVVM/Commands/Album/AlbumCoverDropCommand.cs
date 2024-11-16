//using Maxsys.Core.Helpers;
//using Maxsys.MediaManager.CoreDomain.Commands;
//using Maxsys.MediaManager.MusicContext.ApplicationMVVM.ViewModels;
//using Maxsys.ModelCore.Interfaces.Services;
//using System.Drawing;
//using System.IO;

//namespace Maxsys.MediaManager.MusicContext.ApplicationMVVM.Commands
//{
//    public class AlbumCoverDropCommand : CommandBase
//    {
//        private readonly CreateAlbumViewModel _viewModel;
//        private readonly IDialogService _dialogService;

//        public AlbumCoverDropCommand(CreateAlbumViewModel viewModel, IDialogService dialogService)
//        {
//            _viewModel = viewModel;
//            _dialogService = dialogService;
//        }

//        public override bool CanExecute(object parameter)
//        {
//            var canExecute = parameter is string filePath
//                && Path.GetExtension(filePath).ToLower() == ".jpg";

//            if (!canExecute)
//                _dialogService.ShowMessage(MessageType.Warning,
//                    "Only 'JPG' files are allowed.");

//            return canExecute;
//        }

//        public override void Execute(object parameter)
//        {
//            _viewModel.Model.AlbumCover = Image.FromFile((string)parameter).ImageToBytes();
//        }
//    }
//}