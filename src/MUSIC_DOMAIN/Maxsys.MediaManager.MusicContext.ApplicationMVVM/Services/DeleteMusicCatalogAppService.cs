//using FluentValidation.Results;
//using Maxsys.AppCore;
//using Maxsys.DataCore.Interfaces;
//using Maxsys.MediaManager.MusicContext.ApplicationMVVM.Interfaces.Services;
//using Maxsys.MediaManager.MusicContext.ApplicationMVVM.Models;
//using Maxsys.MediaManager.MusicContext.Domain.Interfaces.Repositories;
//using Maxsys.MediaManager.MusicContext.Domain.Interfaces.Services;
//using Maxsys.MediaManager.MusicContext.Domain.Validators;
//using Microsoft.Extensions.Logging;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;

//namespace Maxsys.MediaManager.MusicContext.ApplicationMVVM.Services
//{
//    public sealed class DeleteMusicCatalogAppService
//        : ApplicationServiceBase, IDeleteMusicCatalogAppService
//    {
//        private readonly ILogger _logger;
//        private readonly IPathService _pathService;
//        private readonly ICatalogService _service;
//        private readonly ICatalogRepository _repository;

//        public DeleteMusicCatalogAppService(IUnitOfWork uow,
//            ILogger<DeleteMusicCatalogAppService> logger,
//            IPathService pathService,
//            ICatalogService service,
//            ICatalogRepository repository)
//            : base(uow)
//        {
//            _logger = logger;
//            _pathService = pathService;
//            _service = service;
//            _repository = repository;
//        }

//        public async Task<ValidationResult> DeleteMusicCatalogAsync(MusicCatalogListModel model)
//        {
//            BeginTransaction();

//            var entityForDeletion = await _repository.GetByIdAsync(model.MusicCatalogId, @readonly: false);

//            // Validate Entity
//            var validator = new CatalogValidator(_repository).SetRulesForDeletion();
//            var validationResult = await validator.ValidateAsync(entityForDeletion);

//            if (!validationResult.IsValid)
//                return validationResult;

//            // Deletion
//            validationResult = await _service.RemoveAsync(entityForDeletion.Id);

//            return validationResult.IsValid
//                ? await CommitAsync()
//                : validationResult;
//        }

//        public async Task<IReadOnlyList<MusicCatalogListModel>> GetMusicCatalogsAsync()
//        {
//            var dtos = await _repository.GetCatalogInfosAsync();

//            return dtos.Select(dto => new MusicCatalogListModel(dto)).ToList();
//        }

//        public async Task DeleteMusicCatalogDirectory(MusicCatalogListModel model)
//        {
//            await Task.Run(() =>
//            {
//                try
//                {
//                    var musicCatalogDirectory = _pathService.GetCatalogDirectory(model.MusicCatalogName);

//                    _logger.LogDebug($"Deleting folder <{musicCatalogDirectory}>.");

//                    System.IO.Directory.Delete(musicCatalogDirectory, true);

//                    _logger.LogWarning($"Folder deleted.");
//                }
//                catch (System.Exception ex)
//                {
//                    _logger.LogError("Folder cannot be deleted:\n{errors}", ex.Message);
//                }
//            });
//        }
//    }
//}