//using FluentValidation.Results;
//using Maxsys.MediaManager.CoreDomain.Commands;
//using Maxsys.MediaManager.MusicContext.ApplicationMVVM.Interfaces.Services;
//using Maxsys.MediaManager.MusicContext.ApplicationMVVM.Models;
//using Maxsys.MediaManager.MusicContext.ApplicationMVVM.ViewModels;
//using Maxsys.ModelCore.Interfaces.Services;
//using Microsoft.Extensions.Logging;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;

//namespace Maxsys.MediaManager.MusicContext.ApplicationMVVM.Commands
//{
//    public class RegisterMusicCommand : CommandBase
//    {
//        private readonly RegisterMusicViewModel _viewModel;

//        private readonly ILogger _logger;
//        private readonly IRegisterMusicAppService _appService;
//        private readonly IDialogService _dialogService;

//        public RegisterMusicCommand(
//            RegisterMusicViewModel viewModel,
//            ILogger logger,
//            IRegisterMusicAppService appService,
//            IDialogService dialogService)
//        {
//            _logger = logger;
//            _viewModel = viewModel;
//            _appService = appService;
//            _dialogService = dialogService;

//            _viewModel.Models.CollectionChanged += (s, e) => OnCanExecuteChanged();
//        }

//        private ValidationResult ValidateModels()
//        {
//            var validationResult = new ValidationResult();

//            _appService.SetTargetFullPaths(_viewModel.Models);

//            var isAllModelsValid = _viewModel.Models.All(m => m.IsValid);
//            if (!isAllModelsValid)
//            {
//                var message = $"Some musics are invalid.";

//                validationResult.AddFailure(message);

//                _logger.LogError(message);
//                _dialogService.ShowMessage(MessageType.Error, message);
//            }

//            return validationResult;
//        }

//        private async Task<IReadOnlyDictionary<CreateMusicModel, ValidationResult>> RegisterMusicsAsync()
//        {
//            var results = new Dictionary<CreateMusicModel, ValidationResult>();
//            var modelsCount = _viewModel.Models.Count;

//            for (int i = 0; i < modelsCount; i++)
//            {
//                var model = _viewModel.Models[i];

//                _dialogService.ShowMessage(MessageType.Status,
//                    $"Registering {i + 1:00} of {modelsCount:00} items: [{model.TargetFullPath}]...");

//                results.Add(model, await _appService.RegisterMusicAsync(model));
//            }

//            return results;
//        }

//        private static IReadOnlyDictionary<CreateMusicModel, ValidationResult> GetValidValidationResults(
//            IReadOnlyDictionary<CreateMusicModel, ValidationResult> validationResults)
//        {
//            return validationResults.Where(kv => kv.Value.IsValid).ToDictionary(kv => kv.Key, kv => kv.Value);
//        }

//        private static IReadOnlyDictionary<CreateMusicModel, ValidationResult> GetInvalidValidationResults(
//            IReadOnlyDictionary<CreateMusicModel, ValidationResult> validationResults)
//        {
//            return validationResults.Where(kv => !kv.Value.IsValid).ToDictionary(kv => kv.Key, kv => kv.Value);
//        }

//        private void ClearSavedModels(IReadOnlyDictionary<CreateMusicModel, ValidationResult> validItems)
//        {
//            foreach (var model in validItems.Keys)
//            {
//                _logger.LogInformation($"Song saved to <{model.TargetFullPath}>.");

//                _viewModel.Models.Remove(model);
//            }
//        }

//        private void SetValidationResultFromRegisterOperationIntoModel(
//            IReadOnlyDictionary<CreateMusicModel, ValidationResult> invalidItems)
//        {
//            foreach (var kv in invalidItems)
//            {
//                var model = kv.Key;
//                var result = kv.Value;

//                model.ValidationResult = result;

//                _logger.LogError($"Song <{model.SourceFullPath}> not saved. Errors:\n\t{result}");
//            }
//        }

//        public override bool CanExecute(object parameter)
//        {
//            return _viewModel.Models is not null
//                && _viewModel.Models.Any()
//                && base.CanExecute(parameter);
//        }

//        public override async void Execute(object parameter)
//        {
//            _logger.LogDebug("Saving musics...");

//            var validationResult = ValidateModels();
//            if (!validationResult.IsValid)
//                return;

//            var validationResults = await RegisterMusicsAsync();

//            var validItems = GetValidValidationResults(validationResults);
//            var invalidItems = GetInvalidValidationResults(validationResults);

//            ClearSavedModels(validItems);
//            SetValidationResultFromRegisterOperationIntoModel(invalidItems);

//            var isAllValid = !invalidItems.Any();
//            if (isAllValid)
//                _dialogService.ShowMessage(MessageType.Information, "All items are saved and moved to library.");
//            else
//                _dialogService.ShowMessage(MessageType.Warning, "One or more items are not saved!");

//            await _viewModel.ReloadMusicsAsync();
//        }
//    }
//}