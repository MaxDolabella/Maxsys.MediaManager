using FluentValidation;
using Maxsys.Core.Helpers;
using Maxsys.MediaManager.CoreDomain.Validators;
using Maxsys.MediaManager.MusicContext.Domain.Entities;
using Maxsys.MediaManager.MusicContext.Domain.Interfaces.Repositories;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Maxsys.MediaManager.MusicContext.Domain.Validators
{
    public class ComposerValidator : EntityValidator<Composer>
    {
        private readonly IComposerRepository _repository;

        public ComposerValidator(IComposerRepository repository)
        {
            _repository = repository;
        }

        #region Private Methods

        private async Task<bool> IsUniqueNameAsync(Composer composer, string name, CancellationToken cancellationToken = default)
        {
            var query = await _repository.FindAsync(x
                => x.Id != composer.Id // Exclude current object from query for updating scenario
                && x.Name == name,
                @readonly: true);

            return !query.Any();
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

        public void AddRuleForName()
        {
            RuleFor(x => x.Name).NotEmpty()
                .Matches(RegexHelper.PATTERN_LETTERS_NUMBERS_SPACES_HYPHENS)
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
}