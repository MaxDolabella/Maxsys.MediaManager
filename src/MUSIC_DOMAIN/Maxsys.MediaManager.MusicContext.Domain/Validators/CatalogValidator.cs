using FluentValidation;
using Maxsys.Core.Helpers;
using Maxsys.MediaManager.MusicContext.Domain.Interfaces.Repositories;

namespace Maxsys.MediaManager.MusicContext.Domain.Validators;

public class CatalogValidator : AbstractValidator<Catalog>
{
    private readonly ICatalogRepository _repository;

    public CatalogValidator(ICatalogRepository repository)
    {
        _repository = repository;
    }

    #region Private Methods

    private async Task<bool> IsUniqueNameAsync(Catalog catalog, string catalogName, CancellationToken token = default)
    {
        return !await _repository.AnyAsync(x
            => x.Id != catalog.Id // Exclude current object from query for updating scenario
            && x.Name == catalogName,
            token);
    }

    private async Task<bool> NotContainsAnyArtist(Catalog catalog, CancellationToken token = default)
    {
        // TODO Usar any aqui???
        var count = await _repository.ArtistCountAsync(catalog.Id, token);

        return count == 0;
    }

    #endregion Private Methods

    #region Validations

    /// <summary>
    /// Gets all validations needed for add new music.<br/>
    /// Uses repository on validations.
    /// </summary>
    /// <returns>the current validator with all validate methods.</returns>
    public CatalogValidator SetRulesForCreation()
    {
        AddRuleForName();
        AddRuleForUniqueName();

        return this;
    }

    public CatalogValidator SetRulesForDeletion()
    {
        AddRuleForId();
        AddRuleForMustNotContainsAnyArtist();

        return this;
    }

    #endregion Validations

    #region Rules

    public void AddRuleForId()
    {
        RuleFor(x => x.Id).NotEmpty();
    }

    /// <summary>
    /// Adds rule to validate <see cref="Catalog.Name"/>.
    /// </summary>
    public CatalogValidator AddRuleForName()
    {
        RuleFor(x => x.Name).NotEmpty()
            .Matches(RegexHelper.GetPattern(RegexHelper.Pattern.Letters | RegexHelper.Pattern.Numbers | RegexHelper.Pattern.Spaces | RegexHelper.Pattern.Hyphen))
                .WithMessage("{PropertyName} must contain only letters, numbers, spaces and hyphens.")
            .MinimumLength(2)
            .MaximumLength(50);

        return this;
    }

    /// <summary>
    /// Adds asynchronous rule to validate wether <see cref="Catalog.Name"/> is unique.
    /// </summary>
    public CatalogValidator AddRuleForUniqueName()
    {
        RuleFor(x => x.Name)
            .MustAsync(IsUniqueNameAsync)
            .WithMessage("'{PropertyName}' {PropertyValue} already exists. Must be unique.");

        return this;
    }

    /// <summary>
    /// Adds a rule used in deletion. To be valid for deletion, the <see cref="Catalog"/>
    /// must not contains any <see cref="Artist"/> in <see cref="Catalog.Artists"/>.
    /// </summary>
    public CatalogValidator AddRuleForMustNotContainsAnyArtist()
    {
        RuleFor(x => x)
            .MustAsync(NotContainsAnyArtist)
                .WithMessage("Song Catalog contains at least one artist. Must not contains any.");

        return this;
    }

    #endregion Rules
}