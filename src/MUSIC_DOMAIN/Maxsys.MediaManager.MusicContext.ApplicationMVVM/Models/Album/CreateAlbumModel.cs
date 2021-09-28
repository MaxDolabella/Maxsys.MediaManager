using Maxsys.Core.Helpers;
using Maxsys.MediaManager.MusicContext.Domain.DTO;
using Maxsys.MediaManager.MusicContext.Domain.ValueObjects;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Maxsys.MediaManager.MusicContext.ApplicationMVVM.Models
{
    public sealed class CreateAlbumModel : ModelBase
    {
        private string _name;
        private int? _year;
        private string _genre;
        private AlbumType _albumType;
        private byte[] _albumCover = Array.Empty<byte>();

        [Browsable(false)]
        public Guid Id { get; } = GuidGen.NewSequentialGuid();

        [Required(AllowEmptyStrings = false, ErrorMessage = "{0} is required.")]
        [Display(Name = nameof(Name), Description = "Is the name of Album.")]
        [MinLength(3, ErrorMessage = "{0} must be at least {1} characters.")]
        [MaxLength(50, ErrorMessage = "{0} must be at most {1} characters.")]
        [RegularExpression(RegexHelper.PATTERN_LETTERS_NUMBERS_PARENTHESIS_COMMA_DOT_SPACES_HYPHENS,
            ErrorMessage = "{0} must contains only letters, numbers and following symbols: (),.- [space]")]
        public string Name
        {
            get => _name;
            set => SetProperty(ref _name, value, true);
        }

        [Range(1500, 2100, ErrorMessage = "{0} must be empty or a number between {1} and {2}")]
        [CustomValidation(typeof(CreateAlbumModel), nameof(ValidateYearByAlbumType))]
        public int? Year
        {
            get => _year;
            set => SetProperty(ref _year, value, true);
        }

        [Required(AllowEmptyStrings = false, ErrorMessage = "{0} is required.")]
        [MinLength(3, ErrorMessage = "{0} must be at least {1} characters.")]
        [MaxLength(50, ErrorMessage = "{0} must be at most {1} characters.")]
        [RegularExpression(RegexHelper.PATTERN_LETTERS_NUMBERS_SPACES_HYPHENS,
            ErrorMessage = "{0} must contains only letters, numbers, spaces and hyphens.")]
        public string Genre
        {
            get => _genre;
            set => SetProperty(ref _genre, value, true);
        }

        [Required]
        [EnumDataType(typeof(AlbumType))]
        public AlbumType AlbumType
        {
            get => _albumType;
            set => SetProperty(ref _albumType, value, true);
        }

        [Required]
        public byte[] AlbumCover
        {
            get => _albumCover;
            set => SetProperty(ref _albumCover, value, true);
        }

        #region Artist

        // Field
        private ArtistInfoDTO _artist;

        // Props
        [Required(ErrorMessage = "{0} is required.")]
        [Browsable(false)]
        public ArtistInfoDTO Artist
        {
            get => _artist;
            set
            {
                if (SetProperty(ref _artist, value, true))
                {
                    OnPropertyChanged(nameof(ArtistName));
                }
            }
        }

        [Browsable(false)]
        public Guid ArtistId => Artist?.ArtistId ?? default;

        [Browsable(false)]
        public string ArtistName => Artist?.ArtistName;

        // Methods
        public void SetArtist(ArtistInfoDTO artist) => Artist = artist;

        #endregion Artist

        #region Custom Validation

        public static ValidationResult ValidateYearByAlbumType(int? year, ValidationContext context)
        {
            // Grab the model instance
            var model = context.ObjectInstance as CreateAlbumModel;
            if (model is null) throw new NullReferenceException();

            // Cross - property validation
            bool isValid = (year, model.AlbumType) switch
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

            return isValid
                ? System.ComponentModel.DataAnnotations.ValidationResult.Success
                : new ValidationResult("'Studio', 'Live', 'Compilation' and 'Bootleg' albums must have a year. 'Undefined', 'Various' and 'Others' albums, must not.");
        }

        #endregion Custom Validation
    }
}