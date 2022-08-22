using Maxsys.MediaManager.MusicContext.Domain.Interfaces.Repositories;
using Maxsys.MediaManager.MusicContext.Domain.Interfaces.Services;

namespace Maxsys.MediaManager.MusicContext.Domain.Services;

public class ArtistService : IArtistService
{
    private readonly IArtistRepository _repository;

    #region CONTRUCTORS

    public ArtistService(IArtistRepository repository)
    {
        _repository = repository;
    }

    #endregion CONTRUCTORS

    //public override async Task<ValidationResult> AddAsync(Artist entity)
    //{
    //    var validationResult = new ValidationResult();
    //    var validator = new ArtistValidator(_repository).SetRulesForNewArtist();

    //    validationResult = await validator.ValidateAsync(entity);
    //    if (!validationResult.IsValid) return validationResult;

    //    var isAdded = await _repository.AddAsync(entity);
    //    if (!isAdded) validationResult.AddFailure($"{nameof(Artist)} could not be added.");

    //    return validationResult;
    //}
}