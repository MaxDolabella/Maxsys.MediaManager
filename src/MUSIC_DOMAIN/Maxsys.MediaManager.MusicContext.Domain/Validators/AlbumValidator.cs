using FluentValidation;
using Maxsys.Core.Helpers;
using Maxsys.MediaManager.MusicContext.Domain.Enums;
using Maxsys.MediaManager.MusicContext.Domain.Interfaces.Repositories;

namespace Maxsys.MediaManager.MusicContext.Domain.Validators;

public class AlbumValidator : AbstractValidator<Album>
{
    private readonly IAlbumRepository _repository;

    public AlbumValidator(IAlbumRepository repository)
    {
        _repository = repository;
    }

    #region Private Methods

    private bool IsValidYearRange(short? albumYear)
    {
        return !albumYear.HasValue || albumYear >= 1500 && albumYear <= 2100;
    }

    private bool IsValidYearGivenAlbumType(Album album, short? albumYear)
    {
        bool isValid = (albumYear, album.AlbumType) switch
        {
            (not null, AlbumTypes.Studio
                or AlbumTypes.Live
                or AlbumTypes.Compilation
                or AlbumTypes.Bootleg) => true,

            (null, AlbumTypes.Undefined
                or AlbumTypes.Various
                or AlbumTypes.Others) => true,

            _ => false
        };

        return isValid;
    }

    private async Task<bool> IsUniqueNameFromArtist(Album album, string name, CancellationToken token = default)
    {
        var artistId = album.Artist?.Id ?? album.ArtistId;

        return !await _repository.AnyAsync(x
            => x.Id == album.Id // Exclude current object from query for updating scenario
            && x.Artist.Id == artistId
            && x.Name == name,
            token);
    }

    private async Task<bool> NotContainsAnySong(Album album, CancellationToken token = default)
    {
        var count = await _repository.SongsCountAsync(album.Id, token);

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
        AddRuleForId();
        RuleForMustNotContainsAnySong();

        return this;
    }

    #endregion Validations

    #region Rules

    public void AddRuleForId()
    {
        RuleFor(x => x.Id).NotEmpty();
    }

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
            .Matches(RegexHelper.GetPattern(RegexHelper.Pattern.Letters | RegexHelper.Pattern.Numbers | RegexHelper.Pattern.Spaces | RegexHelper.Pattern.Hyphen))
                .WithMessage("{PropertyName} must contain only letters, spaces and hyphens.");
    }

    public void RuleForYear()
    {
        RuleFor(x => x.Year).Must(IsValidYearGivenAlbumType)
            .WithMessage("'Studio', 'Live', 'Compilation' and 'Bootleg' albums must have a albumYear. 'Undefined', 'Various' and 'Others' albums, must not.");

        RuleFor(x => x.Year).Must(IsValidYearRange)
            .WithMessage("Must a valid albumYear. Year must be between 1500 and 2100");
    }

    public void RuleForName()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(50)
            .Matches(RegexHelper.GetPattern(RegexHelper.Pattern.Letters | RegexHelper.Pattern.Numbers | RegexHelper.Pattern.Spaces | RegexHelper.Pattern.Hyphen | RegexHelper.Pattern.Parentesis | RegexHelper.Pattern.Commas | RegexHelper.Pattern.Dots))
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
    /// must not contains any <see cref="Song"/> in <see cref="Album.Songs"/>.
    /// </summary>
    public void RuleForMustNotContainsAnySong()
    {
        RuleFor(x => x)
            .MustAsync(NotContainsAnySong)
                .WithMessage("Album contains at least one music. Must not contains any.");
    }

    #endregion Rules
}