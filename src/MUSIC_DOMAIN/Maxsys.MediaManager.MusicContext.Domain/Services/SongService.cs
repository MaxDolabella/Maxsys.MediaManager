using System.Linq.Expressions;
using AutoMapper;
using Maxsys.Core.Interfaces.Data;
using Maxsys.Core.Services;
using Maxsys.MediaManager.MusicContext.Domain.DTO;
using Maxsys.MediaManager.MusicContext.Domain.Interfaces.Mp3;
using Maxsys.MediaManager.MusicContext.Domain.Interfaces.Repositories;
using Maxsys.MediaManager.MusicContext.Domain.Interfaces.Services;
using IOHelper2 = Maxsys.MediaManager.CoreDomain.Helpers.IOHelper2;

namespace Maxsys.MediaManager.MusicContext.Domain.Services;

/// <inheritdoc cref="ISongService"/>
public class SongService : ServiceBase<Song, ISongRepository, Guid>, ISongService
{
    private readonly ITagService _tagService;

    public SongService(ISongRepository repository, IUnitOfWork uow, IMapper mapper, ITagService tagService)
        : base(repository, uow, mapper)
    {
        _tagService = tagService;
    }

    protected override Expression<Func<Song, bool>> IdSelector(Guid id) => x => x.Id == id;

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

    public Task<Song?> GetByPathAsync(Uri songPath, CancellationToken cancellationToken = default)
        => _repository.GetByPathAsync(songPath, cancellationToken);

    public ValueTask<OperationResult> ReplaceToLibraryAsync(Uri replacingFile, Uri libraryFile, CancellationToken cancellationToken = default)
        => IOHelper2.MoveOrOverwriteFileAsync(replacingFile, libraryFile, setAsReadOnly: true);

    public ValueTask<OperationResult> MoveToLibraryAsync(Uri sourceFile, Uri libraryFile, CancellationToken cancellationToken = default)
        => IOHelper2.MoveFileAsync(sourceFile, libraryFile, setAsReadOnly: true, cancellationToken: cancellationToken);

    public async Task<IReadOnlyList<SongRankDTO>> GetAllSongRanksAsync(CancellationToken cancellationToken = default)
        => await _repository.ToListAsync<SongRankDTO>(null, cancellationToken);

    public async Task<IReadOnlyList<SongRankDTO>> GetAllSongRankByRatingRangeAsync(int minimumRating, int maximumRating, CancellationToken cancellationToken = default)
    {
        return await _repository.GetAllSongRankByRatingRangeAsync(minimumRating, maximumRating, cancellationToken);
    }

    public async ValueTask<OperationResult> UpdateSongRankAsync(SongRankDTO songRank, CancellationToken cancellationToken = default)
    {
        if (!songRank.RatingHasChanged())
        {
            return new();
        }

        var entry = await _repository.GetByIdAsync([songRank.Id], true, cancellationToken);
        if (entry is null)
        {
            return new(new Notification(GenericMessages.ITEM_NOT_FOUND, ResultTypes.Warning));
        }

        entry.Classification.UpdateRating(songRank.CurrentRatingPoints);

        await _repository.UpdateAsync(entry, cancellationToken);

        var result = await _uow.SaveChangesAsync(cancellationToken);
        if (!result.IsValid)
        {
            return result;
        }

        if (songRank.Stars10HasChanged())
        {
            var tagResult = await _tagService.WriteRatingAsync(songRank.FullPath, songRank.GetStars10());
            if (!tagResult.IsValid)
            {
                result.AddNotification(new Notification("Rank was updated, but tagging has failed", ResultTypes.Warning));
                result.AddNotifications(tagResult.Notifications);
            }
        }

        return result;
    }
}