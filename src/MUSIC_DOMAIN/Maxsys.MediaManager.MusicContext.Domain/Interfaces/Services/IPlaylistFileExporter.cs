using System;
using FluentValidation.Results;
using Maxsys.Core.Interfaces.Services;

namespace Maxsys.MediaManager.MusicContext.Domain.Interfaces.Services;

public interface IPlaylistFileExporter : IService
{
    Task<OperationResult> ExportFileAsync(IEnumerable<string> songFiles, string destFolder, string playlistName, CancellationToken token = default);
}