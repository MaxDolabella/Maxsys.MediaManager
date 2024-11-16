using AutoMapper;
using Maxsys.Core.Data;
using Maxsys.MediaManager.MusicContext.Domain.Interfaces.Repositories;
using Maxsys.MediaManager.MusicContext.Infra.DataEFCore.Context;

namespace Maxsys.MediaManager.MusicContext.Infra.DataEFCore.Repositories;

/// <summary>
///
/// </summary>
public class AlbumRepository : RepositoryBase<Album>, IAlbumRepository
{
    public AlbumRepository(MusicAppContext context, IMapper mapper) : base(context, mapper)
    { }

    public async Task<IReadOnlyList<string>> GetGenresAsync(CancellationToken token = default)
    {
        return await DbSet.AsNoTracking()
            .Select(entity => entity.Genre)
            .Distinct()
            .OrderBy(genre => genre)
            .ToListAsync(token);
    }

    public async Task<IReadOnlyList<TrackAndTitleDTO>> GetTrackAndTitlesAsync(Guid albumId, CancellationToken token = default)
    {
        return await DbSet.AsNoTracking()
            .Include(a => a.Songs)
            .Where(a => a.Id == albumId)
            .SelectMany(a => a.Songs)
            .Select(m => new TrackAndTitleDTO
            {
                Track = m.TrackNumber,
                Title = m.Title
            })
            .OrderBy(m => m.Track)
            .ThenBy(m => m.Title)
            .ToListAsync(token);
    }

    public async Task<AlbumTagDTO?> GetAlbumTagAsync(Guid albumId, CancellationToken token = default)
    {
        return await DbSet.AsNoTracking()
            .Include(a => a.Artist)
            .Where(a => a.Id == albumId)
            .Select(a => new AlbumTagDTO
            {
                ArtistName = a.Artist.Name,
                AlbumName = a.Name,
                AlbumYear = a.Year,
                AlbumGenre = a.Genre,
                AlbumCover = a.AlbumCover
            })
            .FirstOrDefaultAsync(token);
    }

    public async Task<IReadOnlyList<AlbumInfoDTO>> GetAlbumInfosAsync(CancellationToken token = default)
    {
        return await DbSet.AsNoTracking()
            .Include(e => e.Artist)
            .Select(e => new AlbumInfoDTO
            {
                ArtistId = e.ArtistId,
                ArtistName = e.Artist.Name,
                AlbumId = e.Id,
                AlbumName = e.Name,
                AlbumDirectory = e.AlbumDirectory
            })
            .OrderBy(m => m.AlbumDirectory)
            .ToListAsync(token);
    }

    public async Task<IReadOnlyList<AlbumDetailDTO>> GetAlbumDetailsAsync(CancellationToken token = default)
    {
        return await DbSet.AsNoTracking()
            .Include(e => e.Artist.Catalog)
            .Include(e => e.Songs)
            .Select(e => new AlbumDetailDTO
            {
                AlbumId = e.Id,
                CatalogName = e.Artist.Catalog.Name,
                ArtistName = e.Artist.Name,
                AlbumName = e.Name,
                AlbumType = e.AlbumType,
                AlbumYear = e.Year,
                AlbumDirectory = e.AlbumDirectory,
                AlbumMusicCount = e.Songs.Count()
            })
            .OrderBy(m => m.AlbumDirectory)
            .ToListAsync(token);
    }

    public async Task<int> SongsCountAsync(Guid albumId, CancellationToken token = default)
    {
        return await DbSet.AsNoTracking()
            .Include(e => e.Songs)
            .Where(e => e.Id == albumId)
            .Select(e => e.Songs.Count())
            .FirstOrDefaultAsync(token);
    }
}