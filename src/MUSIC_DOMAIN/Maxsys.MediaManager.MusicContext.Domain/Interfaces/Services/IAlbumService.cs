using FluentValidation.Results;
using Maxsys.Core;
using Maxsys.Core.Interfaces.Services;

namespace Maxsys.MediaManager.MusicContext.Domain.Interfaces.Services;

public interface IAlbumService : IService<Album, Guid>
{
    /// <summary>
    /// Saves an <paramref name="albumCover">album cover</paramref> in a specific <paramref name="directory"/> asynchronous.
    /// </summary>
    /// <param name="directory">Directory to save the album cover.</param>
    /// <param name="albumCover">The album cover to be saved.</param>
    ValueTask<OperationResult> SaveCoverPictureAsync(string directory, byte[] albumCover, CancellationToken token = default);
}