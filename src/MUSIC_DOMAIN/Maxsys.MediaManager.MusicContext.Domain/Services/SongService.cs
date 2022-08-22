using FluentValidation.Results;
using Maxsys.Core.Helpers;
using Maxsys.MediaManager.MusicContext.Domain.DTO;
using Maxsys.MediaManager.MusicContext.Domain.Interfaces.Mp3;
using Maxsys.MediaManager.MusicContext.Domain.Interfaces.Repositories;
using Maxsys.MediaManager.MusicContext.Domain.Interfaces.Services;

namespace Maxsys.MediaManager.MusicContext.Domain.Services;

/// <inheritdoc cref="ISongService"/>
public class SongService : ISongService
{
    private readonly ISongRepository _repository;
    private readonly ITagService _tagService;

    #region CONTRUCTORS

    public SongService(ISongRepository repository, ITagService tagService)
    {
        _repository = repository;
        _tagService = tagService;
    }

    #endregion CONTRUCTORS

    //public override async Task<ValidationResult> AddAsync(Song obj)
    //{
    //    var validationResult = new ValidationResult();
    //    var validator = new SongValidator(_repository).SetRulesForNewMusic();

    //    validationResult = await validator.ValidateAsync(obj);
    //    if (!validationResult.IsValid) return validationResult;

    //    var isAdded = await _repository.AddAsync(obj);
    //    if (!isAdded) validationResult.AddFailure($"{nameof(Song)} could not be added.");

    //    return validationResult;
    //}

    //public override async Task<ValidationResult> UpdateAsync(Song obj)
    //{
    //    var validationResult = new ValidationResult();
    //    var validator = new SongValidator(_repository).SetRulesForUpdateMusic();

    //    validationResult = await validator.ValidateAsync(obj);
    //    if (!validationResult.IsValid) return validationResult;

    //    var isUpdated = await _repository.UpdateAsync(obj);
    //    if (!isUpdated) validationResult.AddFailure($"{nameof(Song)} could not be updated.");

    //    return validationResult;
    //}

    public async Task<Song> GetByPathAsync(string songPath, CancellationToken token = default)
        => await _repository.GetByPathAsync(songPath, token);

    public async ValueTask<ValidationResult> ReplaceToLibraryAsync(string replacingFile, string libraryFile, CancellationToken token = default)
    {
        var validationResult = new ValidationResult();

        if (replacingFile != libraryFile)
        {
            var result = await IOHelper.MoveOrOverwriteFileAsync(replacingFile, libraryFile, setAsReadOnly: true);
            if (!result.IsValid)
                validationResult.AddErrorMessage($"Error replacing the file: {result}");
        }

        return validationResult;
    }

    public async ValueTask<ValidationResult> MoveToLibraryAsync(string sourceFile, string libraryFile, CancellationToken token = default)
    {
        var validationResult = new ValidationResult();

        if (sourceFile != libraryFile)
        {
            var result = await IOHelper.MoveFileAsync(sourceFile, libraryFile, setAsReadOnly: true, cancellationToken: token);
            if (!result.IsValid)
                validationResult.AddErrorMessage($"Error moving the file: {result}");
        }

        return validationResult;
    }

    public async Task<IReadOnlyList<SongRankDTO>> GetAllSongRanksAsync(CancellationToken token = default)
    {
        return await _repository.GetAllSongRanksAsync(token);
    }

    public async Task<IReadOnlyList<SongRankDTO>> GetAllSongRankByRatingRangeAsync(int minimumRating, int maximumRating, CancellationToken token = default)
    {
        return await _repository.GetAllSongRankByRatingRangeAsync(minimumRating, maximumRating, token);
    }

    public async ValueTask<ValidationResult> UpdateSongRankAsync(SongRankDTO songRank, CancellationToken token = default)
    {
        ValidationResult result = new();

        if (songRank.RatingHasChanged())
        {
            if (!await _repository.UpdateSongRankAsync(songRank, token))
                return result.AddErrorMessage("Error while updating rating.");

            if (songRank.Stars10HasChanged())
                result = _tagService.WriteRating(songRank.FullPath, songRank.GetStars10());
        }

        return result;
    }
}