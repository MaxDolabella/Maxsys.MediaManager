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
    public class MusicCatalogValidator : EntityValidator<MusicCatalog>
    {
        private readonly IMusicCatalogRepository _repository;

        public MusicCatalogValidator(IMusicCatalogRepository repository)
        {
            _repository = repository;
        }

        #region Private Methods

        private async Task<bool> IsUniqueNameAsync(MusicCatalog musicCatalog, string name, CancellationToken cancellationToken = default)
        {
            var query = await _repository.FindAsync(x
                => x.Id != musicCatalog.Id // Exclude current object from query for updating scenario
                && x.Name == name,
                @readonly: true);

            return !query.Any();
        }

        private async Task<bool> NotContainsAnyArtist(MusicCatalog musicCatalog, CancellationToken cancellationToken = default)
        {
            int count = await _repository.ArtistsCountAsync(musicCatalog.Id);

            return count == 0;
        }

        #endregion Private Methods

        #region Validations

        /// <summary>
        /// Gets all validations needed for add new music.<br/>
        /// Uses repository on validations.
        /// </summary>
        /// <returns>the current validator with all validate methods.</returns>
        public MusicCatalogValidator SetRulesForCreation()
        {
            AddRuleForName();
            AddRuleForUniqueName();

            return this;
        }

        public MusicCatalogValidator SetRulesForDeletion()
        {
            AddRuleForId();
            AddRuleForMustNotContainsAnyArtist();

            return this;
        }

        #endregion Validations

        #region Rules

        /// <summary>
        /// Adds rule to validate <see cref="MusicCatalog.Name"/>.
        /// </summary>
        public MusicCatalogValidator AddRuleForName()
        {
            RuleFor(x => x.Name).NotEmpty()
                .Matches(RegexHelper.PATTERN_LETTERS_NUMBERS_SPACES_HYPHENS)
                    .WithMessage("{PropertyName} must contain only letters, numbers, spaces and hyphens.")
                .MinimumLength(2)
                .MaximumLength(50);

            return this;
        }

        /// <summary>
        /// Adds asynchronous rule to validate wether <see cref="MusicCatalog.Name"/> is unique.
        /// </summary>
        public MusicCatalogValidator AddRuleForUniqueName()
        {
            RuleFor(x => x.Name)
                .MustAsync(IsUniqueNameAsync)
                .WithMessage("'{PropertyName}' {PropertyValue} already exists. Must be unique.");

            return this;
        }

        /// <summary>
        /// Adds a rule used in deletion. To be valid for deletion, the <see cref="MusicCatalog"/>
        /// must not contains any <see cref="Artist"/> in <see cref="MusicCatalog.Artists"/>.
        /// </summary>
        public MusicCatalogValidator AddRuleForMustNotContainsAnyArtist()
        {
            RuleFor(x => x)
                .MustAsync(NotContainsAnyArtist)
                    .WithMessage("Music Catalog contains at least one artist. Must not contains any.");

            return this;
        }

        #endregion Rules
    }
}