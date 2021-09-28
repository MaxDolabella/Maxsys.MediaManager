using FluentValidation.Results;
using Maxsys.MediaManager.MusicContext.Domain.Entities;
using Maxsys.ModelCore.Interfaces.Services;
using System.Threading.Tasks;

namespace Maxsys.MediaManager.MusicContext.Domain.Interfaces.Services
{
    public interface IMusicService : IServiceBase<Music>
    {
        Task<Music> GetByPathAsync(string musicPath);

        /// <summary>
        /// Asynchronously replaces a file in library to another file
        /// </summary>
        /// <param name="replacingFile">is the current file path of replacing file</param>
        /// <param name="libraryFile">is the destination full path of the file in library to be replaced</param>
        /// <returns></returns>
        ValueTask<ValidationResult> ReplaceToLibraryAsync(string replacingFile, string libraryFile);

        /// <summary>
        /// Asynchronously moves a music file to Library
        /// </summary>
        /// <param name="sourceFile">is the full path of the file before be moved</param>
        /// <param name="libraryFile">is the destination full path of the file</param>
        /// <returns></returns>
        ValueTask<ValidationResult> MoveToLibraryAsync(string sourceFile, string libraryFile);
    }
}