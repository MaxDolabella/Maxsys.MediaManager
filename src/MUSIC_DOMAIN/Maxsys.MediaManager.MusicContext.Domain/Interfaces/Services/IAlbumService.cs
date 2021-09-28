using FluentValidation.Results;
using Maxsys.MediaManager.MusicContext.Domain.Entities;
using Maxsys.ModelCore.Interfaces.Services;
using System.Threading.Tasks;

namespace Maxsys.MediaManager.MusicContext.Domain.Interfaces.Services
{
    public interface IAlbumService : IServiceBase<Album>
    {
        /// <summary>
        /// Saves an <paramref name="albumCover">album cover</paramref> in a specific <paramref name="directory"/> asynchronous.
        /// </summary>
        /// <param name="directory">Directory to save the album cover.</param>
        /// <param name="albumCover">The album cover to be saved.</param>
        Task<ValidationResult> SaveCoverPictureAsync(string directory, byte[] albumCover);
    }
}