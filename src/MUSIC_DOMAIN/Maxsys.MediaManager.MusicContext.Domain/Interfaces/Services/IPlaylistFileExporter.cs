using FluentValidation.Results;
using Maxsys.Core.Interfaces.Services;

namespace Maxsys.MediaManager.MusicContext.Domain.Interfaces.Services;

public interface IPlaylistFileExporter : IService
{
    // TODO destRootFolder: uses URI instead string?
    Task<ValidationResult> ExportFileAsync(IEnumerable<string> songFiles, string destFolder, string playlistName, CancellationToken token = default);
}