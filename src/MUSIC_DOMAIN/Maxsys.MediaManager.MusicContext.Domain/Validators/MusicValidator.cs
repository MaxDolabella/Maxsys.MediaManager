using FluentValidation;
using Maxsys.MediaManager.CoreDomain.Validators;
using Maxsys.MediaManager.MusicContext.Domain.Entities;
using Maxsys.MediaManager.MusicContext.Domain.Interfaces.Repositories;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Maxsys.MediaManager.MusicContext.Domain.Validators
{
    public class MusicValidator : EntityValidator<Music>
    {
        private readonly IMusicRepository _repository;

        public MusicValidator(IMusicRepository repository)
        {
            _repository = repository;
        }

        #region Private Methods

        private async Task<bool> IsUniqueTitleAndTrackInAlbumAsync(Music music, Album album, CancellationToken cancellationToken = default)
        {
            var albumId = album?.Id ?? music.AlbumId;
            var title = music.Title;
            var trackNumber = music.TrackNumber;

            var query = await _repository.FindAsync(x
                => x.Id != music.Id // Exclude current obj from query for updating scenario
                && x.Album.Id == albumId
                && x.Title == title
                && x.TrackNumber == trackNumber,
                @readonly: true);

            return !query.Any();
        }

        private async Task<bool> IsUniqueFullPathAsync(Music music, string fullPath, CancellationToken cancellationToken = default)
        {
            var queryResult = await _repository.FindAsync(x
                => x.Id != music.Id // Exclude current music from query for updating scenario
                && x.FullPath == music.FullPath,
                @readonly: true);

            var isUnique = !queryResult.Any();

            return isUnique;
        }

        #endregion Private Methods

        #region Validators

        /// <summary>
        /// Gets all validations needed for add new music.<br/>
        /// Uses repository on validations.
        /// </summary>
        /// <returns>the current validator with all validate methods.</returns>
        public MusicValidator SetRulesForCreation()
        {
            AddRuleForId();
            AddRuleForFullPath();
            AddRuleForOriginalFileName();
            AddRuleForTitle();
            AddRuleForTrackNumber();
            AddRuleForLyrics();
            AddRuleForComments();

            // Value Objects
            AddRuleForMusicDetails();
            AddRuleForMusicProperties();
            AddRuleForClassification();

            // Navigation
            AddRuleForAlbum();

            // Persistence ASYNC
            AddRuleForWetherIsUniqueFullPathAsync();
            AddRuleForWetherIsUniqueTrackAndTitleInAlbumAsync();

            return this;
        }

        /// <summary>
        /// Gets all validations needed for update a new music.<br/>
        /// Uses repository on validations.
        /// </summary>
        /// <returns>the current validator with all validate methods.</returns>
        public MusicValidator SetRulesForUpdate()
        {
            AddRuleForId();
            AddRuleForFullPath();
            AddRuleForOriginalFileName();
            AddRuleForTitle();
            AddRuleForTrackNumber();
            AddRuleForLyrics();
            AddRuleForComments();

            // Value Objects
            AddRuleForMusicDetails();
            AddRuleForMusicProperties();
            AddRuleForClassification();

            // Navigation
            AddRuleForAlbum();

            // Persistence ASYNC
            AddRuleForWetherIsUniqueFullPathAsync();
            AddRuleForWetherIsUniqueTrackAndTitleInAlbumAsync();

            return this;
        }

        #endregion Validators

        #region Rules

        public void AddRuleForFullPath()
        {
            RuleFor(mediaFile => mediaFile.FullPath).NotEmpty()
                .MaximumLength(260).WithMessage("Path lenght must be lower than 260.");
            // .Must(FileMustExist).WithMessage("File must exist.")
            // .When(music => string.IsNullOrWhiteSpace(music.GetOriginalFilePath()))
            //.Matches(RegexHelper.REGEX_PATTERN_FOR_VALID_FILENAME).WithMessage("{PropertyName} must be a valid filename")
        }

        public void AddRuleForOriginalFileName()
        {
            RuleFor(mediaFile => mediaFile.OriginalFileName).NotEmpty()
                .MaximumLength(100);
        }

        public void AddRuleForTitle()
        {
            RuleFor(music => music.Title).NotEmpty()
                .MaximumLength(100);
        }

        public void AddRuleForTrackNumber()
        {
            RuleFor(music => music.TrackNumber)
                .GreaterThan(0)
                .When(music => music.TrackNumber is not null);
        }

        public void AddRuleForLyrics()
        {
            RuleFor(music => music.Lyrics)
                .MaximumLength(5000)
                .When(music => music.Lyrics is not null);
        }

        public void AddRuleForComments()
        {
            RuleFor(music => music.Comments)
                .MaximumLength(300)
                .When(music => music.Comments is not null);
        }

        public void AddRuleForMusicDetails()
        {
            RuleFor(music => music.MusicDetails).NotNull();
            RuleFor(music => music.MusicDetails.VocalGender).IsInEnum();
            RuleFor(music => music.MusicDetails.CoveredArtist)
                .MaximumLength(50)
                .When(music => music.MusicDetails.CoveredArtist is not null);
            RuleFor(music => music.MusicDetails.FeaturedArtist)
                .MaximumLength(50)
                .When(music => music.MusicDetails.FeaturedArtist is not null);
        }

        public void AddRuleForMusicProperties()
        {
            RuleFor(music => music.MusicProperties).NotNull();
            RuleFor(music => music.MusicProperties.Duration)
                .GreaterThan(new TimeSpan(0, 0, 1)); // bug when a file has 0 sec
            RuleFor(music => music.MusicProperties.BitRate)
                .GreaterThan(96);
        }

        public void AddRuleForClassification()
        {
            RuleFor(music => music.Classification).NotNull();
            RuleFor(music => music.Classification.Rating)
                .GreaterThanOrEqualTo(0);
        }

        public void AddRuleForAlbum()
        {
            RuleFor(music => music.Album).NotNull()
                .When(music => music.AlbumId == default);
        }

        public void AddRuleForWetherIsUniqueTrackAndTitleInAlbumAsync()
        {
            RuleFor(x => x.Album)
                .MustAsync(IsUniqueTitleAndTrackInAlbumAsync)
                .WithMessage("Title '{PropertyValue}' already exists in the album. Must be unique.");
        }

        public void AddRuleForWetherIsUniqueFullPathAsync()
        {
            RuleFor(x => x.FullPath)
                .MustAsync(IsUniqueFullPathAsync)
                .WithMessage("File Path '{PropertyValue}' already exists. Must be unique.");
        }

        #endregion Rules
    }
}