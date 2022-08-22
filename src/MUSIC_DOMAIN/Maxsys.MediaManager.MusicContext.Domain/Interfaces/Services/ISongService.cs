using FluentValidation.Results;
using Maxsys.MediaManager.MusicContext.Domain.DTO;

namespace Maxsys.MediaManager.MusicContext.Domain.Interfaces.Services;

public interface ISongService
{
    Task<Song> GetByPathAsync(string songPath, CancellationToken token = default);

    /// <summary>
    /// Asynchronously replaces a file in library to another file
    /// </summary>
    /// <param name="replacingFile">is the current file path of replacing file</param>
    /// <param name="libraryFile">is the destination full path of the file in library to be replaced</param>
    /// <returns></returns>
    ValueTask<ValidationResult> ReplaceToLibraryAsync(string replacingFile, string libraryFile, CancellationToken token = default);

    /// <summary>
    /// Asynchronously moves a music file to Library
    /// </summary>
    /// <param name="sourceFile">is the full path of the file before be moved</param>
    /// <param name="libraryFile">is the destination full path of the file</param>
    /// <returns></returns>
    ValueTask<ValidationResult> MoveToLibraryAsync(string sourceFile, string libraryFile, CancellationToken token = default);

    Task<IReadOnlyList<SongRankDTO>> GetAllSongRanksAsync(CancellationToken token = default);

    Task<IReadOnlyList<SongRankDTO>> GetAllSongRankByRatingRangeAsync(int minimumRating, int maximumRating, CancellationToken token = default);

    ValueTask<ValidationResult> UpdateSongRankAsync(SongRankDTO songRank, CancellationToken token = default);
}