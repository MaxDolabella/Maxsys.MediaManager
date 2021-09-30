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
    public class ArtistValidator : EntityValidator<Artist>
    {
        private readonly IArtistRepository _repository;

        public ArtistValidator(IArtistRepository repository)
        {
            _repository = repository;
        }

        #region Private Methods

        private async Task<bool> IsUniqueNameInCatalogAsync(Artist artist, string name, CancellationToken cancellationToken = default)
        {
            var musicCatalogId = artist.MusicCatalog?.Id ?? artist.MusicCatalogId;
            var query = await _repository.FindAsync(x
                => x.Id != artist.Id // Exclude current object from query for updating scenario
                && x.MusicCatalog.Id == musicCatalogId
                && x.Name == name,
                @readonly: true);

            return !query.Any();
        }

        private async Task<bool> NotContainsAnyAlbum(Artist artist, CancellationToken cancellationToken = default)
        {
            var count = await _repository.AlbumsCountAsync(artist.Id);

            return count == 0;
        }

        #endregion Private Methods

        #region Rules

        public ArtistValidator AddRuleForName()
        {
            RuleFor(x => x.Name).NotEmpty()
                .Matches(RegexHelper.PATTERN_LETTERS_NUMBERS_SPACES_HYPHENS)
                    .WithMessage("{PropertyName} must contain only letters, numbers, spaces and hyphens.")
                .MaximumLength(50);

            return this;
        }

        public ArtistValidator AddRuleForMusicCatalog()
        {
            RuleFor(x => x.MusicCatalog).NotNull()
                .When(x => x.MusicCatalogId == default);

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
}