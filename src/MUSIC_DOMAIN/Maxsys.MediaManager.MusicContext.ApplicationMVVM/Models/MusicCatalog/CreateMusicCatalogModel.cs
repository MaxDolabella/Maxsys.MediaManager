using Maxsys.Core.Helpers;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Maxsys.MediaManager.MusicContext.ApplicationMVVM.Models
{
    public sealed class CreateMusicCatalogModel : ModelBase
    {
        private string _name;

        [Browsable(false)]
        public Guid Id { get; } = GuidGen.NewSequentialGuid();

        [Display(Name = nameof(Name), Description = "Is the name of Catalog.")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "{0} is required.")]
        [MinLength(3, ErrorMessage = "{0} must be at least {1} characters.")]
        [MaxLength(50, ErrorMessage = "{0} must be at most {1} characters.")]
        [RegularExpression(RegexHelper.PATTERN_LETTERS_NUMBERS_SPACES_HYPHENS,
            ErrorMessage = "{0} must contains only letters, numbers, spaces and hyphens.")]
        public string Name
        {
            get => _name;
            set => SetProperty(ref _name, value, true);
        }
    }
}