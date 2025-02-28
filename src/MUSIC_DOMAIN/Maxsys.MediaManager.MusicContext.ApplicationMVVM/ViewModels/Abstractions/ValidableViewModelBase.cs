using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using Maxsys.Core;
using Maxsys.MediaManager.MusicContext.ApplicationMVVM.Interfaces.ViewModels;

namespace Maxsys.MediaManager.MusicContext.ApplicationMVVM.ViewModels.Abstractions;

/// <summary>
/// Inherits <see cref="ObservableValidator"/>.
/// <br/>
/// Contains <see cref="OperationResult">ValidationResult</see>,
/// <see cref="IsValid">IsValid</see> and <see cref="Errors">Errors</see>
/// properties.
/// </summary>
public abstract class ValidableViewModelBase : ObservableValidator, IValidableViewModel
{
    private IEnumerable<string>? _errors;

    [Browsable(false)]
    public bool IsValid
    {
        get
        {
            ValidateAllProperties();

            var errorrArray = GetErrors();

            _errors = errorrArray.Any()
                ? errorrArray.Where(vr => !string.IsNullOrEmpty(vr.ErrorMessage)).Select(vr => vr.ErrorMessage!)
                : null;

            OnPropertyChanged(nameof(Errors));
            OnPropertyChanged();

            return _errors?.Any() != true;
        }
    }

    public string Errors => _errors is null ? string.Empty : string.Join(" | ", _errors);
}