using FluentValidation.Results;

namespace Maxsys.MediaManager.MusicContext.ApplicationMVVM.Interfaces.ViewModels;

public interface IValidableViewModel
{
    public ValidationResult? ValidationResult { get; set; }

    public bool IsValid { get; }

    public string Errors { get; }
}