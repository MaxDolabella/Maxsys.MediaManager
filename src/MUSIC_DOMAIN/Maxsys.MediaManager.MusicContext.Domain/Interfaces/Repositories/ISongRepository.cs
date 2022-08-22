using FluentValidation.Results;
using Maxsys.MediaManager.MusicContext.Domain.DTO;
using Maxsys.ModelCore.Interfaces.Repositories;

namespace Maxsys.MediaManager.MusicContext.Domain.Interfaces.Repositories;

public interface ISongRepository : IRepositoryBase<Song>
{
    /// <summary>
    /// Asynchronously checks whether a music is already registered, given a music path
    /// </summary>
    /// <param name="songPath">is the music path to find.</param>
    /// <returns><see langword="true"/> if music exists in music records. Othewise, <see langword="false"/>.</returns>
    Task<bool> PathExistsAsync(string songPath, CancellationToken token = default);

    // TODO Obter o DTO
    Task<Song?> GetByPathAsync(string songPath, CancellationToken token = default);

    /// <summary>
    /// Asynchronously returns a readonly list of <see cref="SongInfoDTO"/>.
    /// </summary>
    Task<IReadOnlyList<SongInfoDTO>> GetSongInfosAsync(CancellationToken token = default);

    /// <summary>
    /// Asynchronously returns a readonly list of <see cref="SongDetailDTO"/>.
    /// </summary>
    Task<IReadOnlyList<SongDetailDTO>> GetSongDetailsAsync(CancellationToken token = default);

    Task<IReadOnlyList<SongRankDTO>> GetAllSongRanksAsync(CancellationToken token = default);

    Task<IReadOnlyList<SongRankDTO>> GetAllSongRankByRatingRangeAsync(int minimumRating, int maximumRating, CancellationToken token = default);

    Task<bool> UpdateSongRankAsync(SongRankDTO songRank, CancellationToken token = default);
}