using Maxsys.Core.Helpers;
using Maxsys.MediaManager.MusicContext.Domain.DTO;
using Maxsys.MediaManager.MusicContext.Domain.ValueObjects;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Maxsys.MediaManager.MusicContext.ApplicationMVVM.Models
{
    public sealed class CreateMusicModel : ModelBase
    {
        #region FIELDS

        private int? _trackNumber;
        private string _title;
        private string _lyrics;
        private string _comments;
        private byte _rating;
        private bool _isBonusTrack;
        private VocalGender _vocalGender;
        private string _coveredArtist;
        private string _featuredArtist;

        #endregion FIELDS

        [Browsable(false)]
        public Guid Id { get; } = GuidGen.NewSequentialGuid();

        [RegularExpression(RegexHelper.PATTERN_NUMBERS, ErrorMessage = "Track Number must contains only numbers.")]
        [Range(1, 9999, ErrorMessage = "Track Number must be a number between {1} and {2}.")]
        [DisplayName("TRACK")]
        public int? TrackNumber
        {
            get => _trackNumber;
            set => SetProperty(ref _trackNumber, value, true);
        }

        [Required(AllowEmptyStrings = false, ErrorMessage = "{0} is required.")]
        [StringLength(100, ErrorMessage = "{0} must be at most {1} characters.")]
        [DisplayName("TITLE")]
        public string Title
        {
            get => _title;
            set => SetProperty(ref _title, value, true);
        }

        [Browsable(false)]
        [MaxLength(5000, ErrorMessage = "{0} must be at most {1} characters.")]
        public string Lyrics
        {
            get => _lyrics;
            set => SetProperty(ref _lyrics, value, true);
        }

        [Browsable(false)]
        [MaxLength(300, ErrorMessage = "{0} must be at most {1} characters.")]
        public string Comments
        {
            get => _comments;
            set => SetProperty(ref _comments, value, true);
        }

        [Required]
        [RegularExpression(RegexHelper.PATTERN_NUMBERS, ErrorMessage = "{0} must contains only numbers.")]
        [Range(0, 10, ErrorMessage = "{0} must be a number between {1} and {2}.")]
        [DisplayName("RATING (0-10)")]
        public byte Rating
        {
            get => _rating;
            set => SetProperty(ref _rating, value, true);
        }

        [Required]
        [DisplayName("IS BONUS TRACK?")]
        public bool IsBonusTrack
        {
            get => _isBonusTrack;
            set => SetProperty(ref _isBonusTrack, value, true);
        }

        [Required]
        [EnumDataType(typeof(VocalGender))]
        [DisplayName("VOCAL GENDER")]
        public VocalGender VocalGender
        {
            get => _vocalGender;
            set => SetProperty(ref _vocalGender, value, true);
        }

        [MaxLength(50, ErrorMessage = "Covered Artist must be at most {1} characters.")]
        [DisplayName("COVERED ARTIST")]
        public string CoveredArtist
        {
            get => _coveredArtist;
            set => SetProperty(ref _coveredArtist, value, true);
        }

        [MaxLength(50, ErrorMessage = "Featured Artist must be at most {1} characters.")]
        [DisplayName("FEATURED BY")]
        public string FeaturedArtist
        {
            get => _featuredArtist;
            set => SetProperty(ref _featuredArtist, value, true);
        }

        #region Paths

        // Fields
        //private string _sourceFullPath;
        //private string _targetFullPath;

        // Props
        [Required]
        [Browsable(false)]
        public string SourceFullPath { get; init; }

        //{
        //    get => _sourceFullPath;
        //    set => SetProperty(ref _sourceFullPath, value, true);
        //}

        [Required]
        [Browsable(false)]
        [Unlike(nameof(SourceFullPath))]
        public string TargetFullPath { get; private set; }

        //{
        //    get => _targetFullPath;
        //    set => SetProperty(ref _targetFullPath, value, true);
        //}

        // Methods
        public void SetTargetFullPath(string value) => TargetFullPath = value;

        #endregion Paths

        #region Album

        // Field
        private AlbumInfoDTO _album;

        // Props
        [Browsable(false)]
        public AlbumInfoDTO Album
        {
            get => _album;
            set
            {
                if (SetProperty(ref _album, value, true))
                {
                    //OnPropertyChanged(nameof(AlbumId));
                    OnPropertyChanged(nameof(AlbumName));
                }
            }
        }

        [Browsable(false)]
        public Guid AlbumId => Album is null ? default : Album.AlbumId;

        [Required(ErrorMessage = "{0} is required.")]
        [DisplayName("ALBUM NAME")]
        public string AlbumName => Album is null ? default : Album.AlbumName;

        // Methods
        public void SetAlbum(AlbumInfoDTO album) => Album = album;

        #endregion Album

        public void SetVocalGender(VocalGender vocalGender) => VocalGender = vocalGender;
    }
}