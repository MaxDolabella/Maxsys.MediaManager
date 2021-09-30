using CommunityToolkit.Mvvm.ComponentModel;
using FluentValidation.Results;
using System.ComponentModel;
using System.Linq;

namespace Maxsys.MediaManager.MusicContext.ApplicationMVVM.Models
{
    public abstract class ValidableModelBase : ObservableValidator //ObservableObject
    {
        private ValidationResult _validationResult;

        #region Validation

        [Browsable(false)]
        public ValidationResult ValidationResult
        {
            get => _validationResult;
            set => SetProperty(ref _validationResult, value, nameof(Errors));
        }

        [Browsable(false)]
        public bool IsValid
        {
            get
            {
                ValidateAllProperties();

                ValidationResult = new ValidationResult(GetErrors()
                    .Select(x => new ValidationFailure(string.Empty, x.ErrorMessage)));

                OnPropertyChanged(nameof(Errors));
                OnPropertyChanged();

                return ValidationResult.IsValid;
            }
        }

        public string Errors => ValidationResult is null ? string.Empty : string.Join(" || ", ValidationResult.ErrorMessagesAsEnumerable());

        #endregion Validation
    }
}