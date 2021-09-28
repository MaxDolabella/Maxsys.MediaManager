using FluentValidation.Results;
using Maxsys.Core.Helpers;
using Maxsys.MediaManager.MusicContext.Domain.Entities;
using Maxsys.MediaManager.MusicContext.Domain.Interfaces.Repositories;
using Maxsys.MediaManager.MusicContext.Domain.Interfaces.Services;
using Maxsys.MediaManager.MusicContext.Domain.Validators;
using Maxsys.ModelCore.Services;
using System.Threading.Tasks;

namespace Maxsys.MediaManager.MusicContext.Domain.Services
{
    /// <inheritdoc cref="IMusicService"/>
    public class MusicService : ServiceBase<Music>, IMusicService
    {
        private readonly IMusicRepository _repository;

        #region CONTRUCTORS

        public MusicService(IMusicRepository repository) : base(repository)
        {
            _repository = repository;
        }

        #endregion CONTRUCTORS

        //public override async Task<ValidationResult> AddAsync(Music obj)
        //{
        //    var validationResult = new ValidationResult();
        //    var validator = new MusicValidator(_repository).SetRulesForNewMusic();

        //    validationResult = await validator.ValidateAsync(obj);
        //    if (!validationResult.IsValid) return validationResult;

        //    var isAdded = await _repository.AddAsync(obj);
        //    if (!isAdded) validationResult.AddFailure($"{nameof(Music)} could not be added.");

        //    return validationResult;
        //}

        //public override async Task<ValidationResult> UpdateAsync(Music obj)
        //{
        //    var validationResult = new ValidationResult();
        //    var validator = new MusicValidator(_repository).SetRulesForUpdateMusic();

        //    validationResult = await validator.ValidateAsync(obj);
        //    if (!validationResult.IsValid) return validationResult;

        //    var isUpdated = await _repository.UpdateAsync(obj);
        //    if (!isUpdated) validationResult.AddFailure($"{nameof(Music)} could not be updated.");

        //    return validationResult;
        //}

        public async Task<Music> GetByPathAsync(string musicPath)
            => await _repository.GetByPathAsync(musicPath);

        public async ValueTask<ValidationResult> ReplaceToLibraryAsync(string replacingFile, string libraryFile)
        {
            var validationResult = new ValidationResult();

            if (replacingFile != libraryFile)
            {
                var result = await IOHelper.MoveOrOverwriteFileAsync(replacingFile, libraryFile, setAsReadOnly: true);
                if (!result.IsValid)
                    validationResult.AddFailure($"{nameof(ReplaceToLibraryAsync)}"
                        , $"Error replacing the file: {result.ToString()}");
            }

            return validationResult;
        }

        public async ValueTask<ValidationResult> MoveToLibraryAsync(string sourceFile, string libraryFile)
        {
            var validationResult = new ValidationResult();

            if (sourceFile != libraryFile)
            {
                var result = await IOHelper.MoveFileAsync(sourceFile, libraryFile, setAsReadOnly: true);
                if (!result.IsValid)
                    validationResult.AddFailure($"{nameof(MoveToLibraryAsync)}"
                        , $"Error moving the file: {result.ToString()}");
            }

            return validationResult;
        }
    }
}