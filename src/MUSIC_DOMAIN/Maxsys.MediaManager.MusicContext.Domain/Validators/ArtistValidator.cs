using FluentValidation;
using Maxsys.Core.Helpers;
using Maxsys.MediaManager.MusicContext.Domain.Interfaces.Repositories;

namespace Maxsys.MediaManager.MusicContext.Domain.Validators;

public class ArtistValidator : AbstractValidator<Artist>
{
    private readonly IArtistRepository _repository;

    public ArtistValidator(IArtistRepository repository)
    {
        _repository = repository;
    }

    #region Private Methods

    private async Task<bool> IsUniqueNameInCatalogAsync(Artist artist, string artistName, CancellationToken token = default)
    {
        var musicCatalogId = artist.Catalog?.Id ?? artist.CatalogId;
        return !await _repository.AnyAsync(x
            => x.Id != artist.Id // Exclude current object from query for updating scenario
            && x.Catalog.Id == musicCatalogId
            && x.Name == artistName,
            token);
    }

    private async Task<bool> NotContainsAnyAlbum(Artist artist, CancellationToken token = default)
    {
        // TODO Any??
        var count = await _repository.AlbumsCountAsync(artist.Id, token);

        return count == 0;
    }

    #endregion Private Methods

    #region Rules

    public void AddRuleForId()
    {
        RuleFor(x => x.Id).NotEmpty();
    }

    public ArtistValidator AddRuleForName()
    {
        RuleFor(x => x.Name).NotEmpty()
            .Matches(RegexHelper.GetPattern(RegexHelper.Pattern.Letters | RegexHelper.Pattern.Numbers | RegexHelper.Pattern.Spaces | RegexHelper.Pattern.Hyphen))
                .WithMessage("{PropertyName} must contain only letters, numbers, spaces and hyphens.")
            .MaximumLength(50);

        return this;
    }

    public ArtistValidator AddRuleForMusicCatalog()
    {
        RuleFor(x => x.Catalog).NotNull()
            .When(x => x.CatalogId == default);

        return this;
    }

    public ArtistValidator AddRuleForUniqueNameInMusicCatalog()
    {
        RuleFor(x => x.Name)
            .MustAsync(IsUniqueNameInCatalogAsync)
                .WithMessage("'{PropertyName}' {PropertyValue} already exists. Must be unique.");

        return this;
    }

    /// <summary>
    /// Adds a rule used in deletion. To be valid for deletion, the <see cref="Artist"/>
    /// must not contains any <see cref="Album"/> in <see cref="Artist.Albums"/>.
    /// </summary>
    public ArtistValidator AddRuleForMustNotContainsAnyAlbum()
    {
        RuleFor(x => x)
            .MustAsync(NotContainsAnyAlbum)
                .WithMessage("Artist contains at least one album. Must not contains any.");

        return this;
    }

    #endregion Rules

    #region Validations

    public ArtistValidator SetRulesForCreation()
    {
        AddRuleForName();
        AddRuleForMusicCatalog();
        AddRuleForUniqueNameInMusicCatalog();

        return this;
    }

    public ArtistValidator SetRulesForDeletion()
    {
        AddRuleForId();
        AddRuleForMustNotContainsAnyAlbum();

        return this;
    }

    #endregion Validations
}