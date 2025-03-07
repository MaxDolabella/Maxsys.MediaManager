using FluentValidation.Results;
using Maxsys.Core;
using Maxsys.Core.Interfaces.Services;
using Maxsys.MediaManager.MusicContext.Domain.DTO;

namespace Maxsys.MediaManager.MusicContext.Domain.Interfaces.Services;

public interface ISongService : IService<Song, Guid>
{
    Task<Song?> GetByPathAsync(string songPath, CancellationToken cancellationToken = default);

    /// <summary>
    /// Asynchronously replaces a file in library to another file
    /// </summary>
    /// <param name="replacingFile">is the current file path of replacing file</param>
    /// <param name="libraryFile">is the destination full path of the file in library to be replaced</param>
    /// <returns></returns>
    ValueTask<OperationResult> ReplaceToLibraryAsync(string replacingFile, string libraryFile, CancellationToken cancellationToken = default);

    /// <summary>
    /// Asynchronously moves a music file to Library
    /// </summary>
    /// <param name="sourceFile">is the full path of the file before be moved</param>
    /// <param name="libraryFile">is the destination full path of the file</param>
    /// <returns></returns>
    ValueTask<OperationResult> MoveToLibraryAsync(string sourceFile, string libraryFile, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<SongRankDTO>> GetAllSongRanksAsync(CancellationToken cancellationToken = default);

    Task<IReadOnlyList<SongRankDTO>> GetAllSongRankByRatingRangeAsync(int minimumRating, int maximumRating, CancellationToken cancellationToken = default);

    ValueTask<OperationResult> UpdateSongRankAsync(SongRankDTO songRank, CancellationToken cancellationToken = default);
}