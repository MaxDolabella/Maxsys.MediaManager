using FluentValidation;
using Maxsys.Core.Helpers;
using Maxsys.MediaManager.MusicContext.Domain.Interfaces.Repositories;

namespace Maxsys.MediaManager.MusicContext.Domain.Validators;

public class PlaylistValidator : AbstractValidator<Playlist>
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

    public void AddRuleForId()
    {
        RuleFor(x => x.Id).NotEmpty();
    }

    public void RuleForName()
    {
        RuleFor(x => x.Name).NotEmpty()
            .Matches(RegexHelper.GetPattern(RegexHelper.Pattern.Letters | RegexHelper.Pattern.Numbers | RegexHelper.Pattern.Spaces | RegexHelper.Pattern.Hyphen))
                .WithMessage("{PropertyName} must contain only letters, numbers, spaces and hyphens.")
            .Length(3, 20);
    }

    public void RuleForUniqueName()
    {
        RuleFor(x => x.Name)
            .MustAsync(IsUniqueNameAsync)
                .WithMessage("'{PropertyName}' {PropertyValue} already exists. Must be unique.");
    }

    // TODO rever
    public void RuleForItens()
    {
        //RuleFor(x => x.Items)
        //    .NotEmptyList()
        //    .OnlyUniqueItens(x => x.SongId)
        //        .WithMessage("'{PropertyName}' {PropertyValue} already exists. Must be unique.");
    }

    #endregion Rules
}