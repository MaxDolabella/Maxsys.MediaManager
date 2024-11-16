using FluentValidation;
using Maxsys.Core.Helpers;
using Maxsys.MediaManager.MusicContext.Domain.Interfaces.Repositories;

namespace Maxsys.MediaManager.MusicContext.Domain.Validators;

public class ComposerValidator : AbstractValidator<Composer>
{
    private readonly IComposerRepository _repository;

    public ComposerValidator(IComposerRepository repository)
    {
        _repository = repository;
    }

    #region Private Methods

    private async Task<bool> IsUniqueNameAsync(Composer composer, string name, CancellationToken token = default)
    {
        return !await _repository.AnyAsync(x
            => x.Id != composer.Id // Exclude current object from query for updating scenario
            && x.Name == name,
            token);
    }

    #endregion Private Methods

    #region Validations

    public ComposerValidator SetRulesForCreation()
    {
        AddRuleForName();
        AddRuleForUniqueName();

        return this;
    }

    #endregion Validations

    #region Rules

    public void AddRuleForId()
    {
        RuleFor(x => x.Id).NotEmpty();
    }

    public void AddRuleForName()
    {
        RuleFor(x => x.Name).NotEmpty()
            .Matches(RegexHelper.GetPattern(RegexHelper.Pattern.Letters | RegexHelper.Pattern.Numbers | RegexHelper.Pattern.Spaces | RegexHelper.Pattern.Hyphen))
                .WithMessage("{PropertyName} must contain only letters, numbers, spaces and hyphens.")
            .MinimumLength(2)
            .MaximumLength(30);
    }

    public void AddRuleForUniqueName()
    {
        RuleFor(x => x.Name)
            .MustAsync(IsUniqueNameAsync)
                .WithMessage("'{PropertyName}' {PropertyValue} already exists. Must be unique.");
    }

    #endregion Rules
}