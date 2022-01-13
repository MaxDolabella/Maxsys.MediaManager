using FluentValidation.Results;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Maxsys.MediaManager.MusicContext.Domain.Interfaces.Services
{
    public interface IPlaylistFileExporter
    {
        // TODO destRootFolder: uses URI instead string?
        Task<ValidationResult> ExportFile(IEnumerable<string> musicFiles, string destFolder, string playlistName);
    }
}