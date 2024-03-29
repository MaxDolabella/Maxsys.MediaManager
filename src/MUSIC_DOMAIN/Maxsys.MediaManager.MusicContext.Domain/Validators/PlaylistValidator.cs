using FluentValidation;
using Maxsys.Core.Helpers;
using Maxsys.MediaManager.CoreDomain.Validators;
using Maxsys.MediaManager.MusicContext.Domain.Interfaces.Repositories;

namespace Maxsys.MediaManager.MusicContext.Domain.Validators;

public class PlaylistValidator : EntityValidator<Playlist>
{
    private readonly IPlaylistRepository _repository;

    public PlaylistValidator(IPlaylistRepository repository)
    {
        _repository = repository;
    }

    #region Private Methods

    private async Task<bool> IsUniqueNameAsync(Playlist playlist, string name, CancellationToken token = default)
    {
        return !await _repository.AnyAsync(x
            => x.Id != playlist.Id // Exclude current object from query for updating scenario
            && x.Name == name,
            token);
    }

    #endregion Private Methods

    #region Validations

    public PlaylistValidator SetRulesForCreation()
    {
        RuleForName();
        RuleForUniqueName();
        RuleForItens();

        return this;
    }

    #endregion Validations

    #region Rules

    public void RuleForName()
    {
        RuleFor(x => x.Name).NotEmpty()
            .Matches(RegexHelper.PATTERN_LETTERS_NUMBERS_SPACES_HYPHENS)
                .WithMessage("{PropertyName} must contain only letters, numbers, spaces and hyphens.")
            .Length(3, 20);
    }

    public void RuleForUniqueName()
    {
        RuleFor(x => x.Name)
            .MustAsync(IsUniqueNameAsync)
                .WithMessage("'{PropertyName}' {PropertyValue} already exists. Must be unique.");
    }

    public void RuleForItens()
    {
        RuleFor(x => x.Items)
            .NotEmptyList()
            .OnlyUniqueItens(x => x.SongId)
                .WithMessage("'{PropertyName}' {PropertyValue} already exists. Must be unique.");
    }

    #endregion Rules
}