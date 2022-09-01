using CommunityToolkit.Mvvm.ComponentModel;
using FluentValidation.Results;
using Maxsys.MediaManager.MusicContext.ApplicationMVVM.Interfaces.ViewModels;
using System.ComponentModel;
using System.Linq;

namespace Maxsys.MediaManager.MusicContext.ApplicationMVVM.ViewModels.Abstractions;

/// <summary>
/// Inherits <see cref="ObservableValidator"/>.
/// <br/>
/// Contains <see cref="ValidationResult">ValidationResult</see>,
/// <see cref="IsValid">IsValid</see> and <see cref="Errors">Errors</see>
/// properties.
/// </summary>
public abstract class ValidableViewModelBase : ObservableValidator, IValidableViewModel
{
    private ValidationResult? _validationResult;

    #region Validation

    [Browsable(false)]
    public ValidationResult? ValidationResult
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

            ValidationResult = new ValidationResult(
                GetErrors().Select(x => new ValidationFailure(string.Empty, x.ErrorMessage)));

            OnPropertyChanged(nameof(Errors));
            OnPropertyChanged();

            return ValidationResult.IsValid;
        }
    }

    public string Errors => ValidationResult is null ? string.Empty : string.Join(" | ", ValidationResult.GetErrorMessages());

    #endregion Validation
}