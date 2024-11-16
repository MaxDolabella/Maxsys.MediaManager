//using FluentValidation.Results;
//using Maxsys.AppCore;
//using Maxsys.DataCore.Interfaces;
//using Maxsys.MediaManager.MusicContext.ApplicationMVVM.Interfaces.Services;
//using Maxsys.MediaManager.MusicContext.ApplicationMVVM.Models;
//using Maxsys.MediaManager.MusicContext.ApplicationMVVM.ViewModels.Catalog;
//using Maxsys.MediaManager.MusicContext.Domain.Factories;
//using Maxsys.MediaManager.MusicContext.Domain.Interfaces.Repositories;
//using Maxsys.MediaManager.MusicContext.Domain.Interfaces.Services;
//using Maxsys.MediaManager.MusicContext.Domain.Validators;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;

//namespace Maxsys.MediaManager.MusicContext.ApplicationMVVM.Services
//{
//    public sealed class CreateArtistAppService
//        : ApplicationServiceBase, ICreateArtistAppService
//    {
//        private readonly IArtistService _service;
//        private readonly IArtistRepository _artistRepository;
//        private readonly ICatalogRepository _musicCatalogRepository;

//        public CreateArtistAppService(IUnitOfWork uow,
//            IArtistService service,
//            IArtistRepository repository,
//            ICatalogRepository musicCatalogRepository)
//            : base(uow)
//        {
//            _service = service;
//            _artistRepository = repository;
//            _musicCatalogRepository = musicCatalogRepository;
//        }

//        public async Task<ValidationResult> AddNewArtistAsync(CreateArtistModel model)
//        {
//            BeginTransaction();

//            var entity = ArtistFactory.Create(model.Name, model.Id, model.MusicCatalogId);
//            var validator = new ArtistValidator(_artistRepository)
//                .SetRulesForCreation();

//            var validationResult = await _service.AddAsync(entity, validator);
//            return validationResult.IsValid
//                ? await CommitAsync()
//                : await Task.FromResult(validationResult);
//        }

//        public async Task<IReadOnlyList<CatalogDetailViewModel>> GetMusicCatalogsAsync()
//        {
//            var dtos = await _musicCatalogRepository.GetCatalogDetailsAsync();

//            return dtos.Select(dto => new CatalogDetailViewModel(dto)).ToList();
//        }

//        public async Task<IReadOnlyList<ArtistInfoModel>> GetArtistsAsync()
//        {
//            var dtos = await _artistRepository.GetArtistDetailsAsync();

//            return dtos.Select(dto => new ArtistInfoModel(dto)).ToList();
//        }
//    }
//}