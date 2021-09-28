using FluentValidation;
using Maxsys.Core.Helpers;
using Maxsys.MediaManager.CoreDomain.Validators;
using Maxsys.MediaManager.MusicContext.Domain.Entities;
using Maxsys.MediaManager.MusicContext.Domain.Interfaces.Repositories;
using Maxsys.MediaManager.MusicContext.Domain.ValueObjects;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Maxsys.MediaManager.MusicContext.Domain.Validators
{
    public class AlbumValidator : EntityValidator<Album>
    {
        private readonly IAlbumRepository _repository;

        public AlbumValidator(IAlbumRepository repository)
        {
            _repository = repository;
        }

        #region Private Methods

        private bool IsValidYearRange(int? year)
        {
            return !year.HasValue || (year >= 1500 && year <= 2100);
        }

        private bool IsValidYearGivenAlbumType(Album album, int? year)
        {
            bool isValid = (year, album.AlbumType) switch
            {
                (not null, AlbumType.Studio
                    or AlbumType.Live
                    or AlbumType.Compilation
                    or AlbumType.Bootleg) => true,

                (null, AlbumType.Undefined
                    or AlbumType.Various
                    or AlbumType.Others) => true,

                _ => false
            };

            return isValid;
        }

        private async Task<bool> IsUniqueNameFromArtist(Album album, string name, CancellationToken cancellationToken = default)
        {
            var artistId = album.Artist?.Id ?? album.ArtistId;

            var query = await _repository.FindAsync(x
                => x.Id == album.Id // Exclude current object from query for updating scenario
                && x.Artist.Id == artistId
                && x.Name == name,
                @readonly: true);

            return !query.Any();
        }

        private async Task<bool> NotContainsAnyMusic(Album album, CancellationToken cancellationToken = default)
        {
            var count = await _repository.MusicsCountAsync(album.Id);

            return count == 0;
        }

        #endregion Private Methods

        #region Validations

        public AlbumValidator SetRulesForCreation()
        {
            RuleForName();
            RuleForYear();
            RuleForGenre();
            RuleForAlbumType();
            RuleForAlbumCover();
            RuleForAlbumDirectory();
            RuleForArtist();
            RuleForUniqueNameFromArtist();

            return this;
        }

        public AlbumValidator SetRulesForDeletion()
        {
            RuleForId();
            RuleForMustNotContainsAnyMusic();

            return this;
        }

        #endregion Validations

        #region Rules

        public void RuleForArtist()
        {
            RuleFor(x => x.Artist).NotNull()
                .When(x => x.ArtistId == default);
        }

        public void RuleForAlbumCover()
        {
            RuleFor(x => x.AlbumCover).NotNull();
        }

        public void RuleForAlbumType()
        {
            RuleFor(x => x.AlbumType).NotNull().IsInEnum();
        }

        public void RuleForGenre()
        {
            RuleFor(x => x.Genre).NotEmpty().MaximumLength(50)
                .Matches(RegexHelper.PATTERN_LETTERS_NUMBERS_SPACES_HYPHENS)
                    .WithMessage("{PropertyName} must contain only letters, spaces and hyphens.");
        }

        public void RuleForYear()
        {
            RuleFor(x => x.Year).Must(IsValidYearGivenAlbumType)
                .WithMessage("'Studio', 'Live', 'Compilation' and 'Bootleg' albums must have a year. 'Undefined', 'Various' and 'Others' albums, must not.");

            RuleFor(x => x.Year).Must(IsValidYearRange)
                .WithMessage("Must a valid year. Year must be between 1500 and 2100");
        }

        public void RuleForName()
        {
            RuleFor(x => x.Name).NotEmpty().MaximumLength(50)
                .Matches(RegexHelper.PATTERN_LETTERS_NUMBERS_PARENTHESIS_COMMA_DOT_SPACES_HYPHENS)
                    .WithMessage("{PropertyName} must contain only letters, numbers, and following symbols: (),.- [space]");
        }

        public void RuleForAlbumDirectory()
        {
            RuleFor(x => x.AlbumDirectory).NotEmpty()
                .MaximumLength(200)
                    .WithMessage("{PropertyName} lenght must be lower than 200.");
        }

        public void RuleForUniqueNameFromArtist()
        {
            RuleFor(x => x.Name)
                .MustAsync(IsUniqueNameFromArtist)
                    .WithMessage("'{PropertyValue}' album already exists for the Artist. Must be unique.");
        }

        /// <summary>
        /// Adds a rule used in deletion. To be valid for deletion, the <see cref="Album"/> 
        /// must not contains any <see cref="Music"/> in <see cref="Album.Musics"/>.
        /// </summary>
        public void RuleForMustNotContainsAnyMusic()
        {
            RuleFor(x => x)
                .MustAsync(NotContainsAnyMusic)
                    .WithMessage("Album contains at least one music. Must not contains any.");
        }

        #endregion Rules
    }
}