using AutoMapper;
using Maxsys.Core.Data;
using Maxsys.MediaManager.MusicContext.Domain.Interfaces.Repositories;
using Maxsys.MediaManager.MusicContext.Infra.DataEFCore.Context;

namespace Maxsys.MediaManager.MusicContext.Infra.DataEFCore.Repositories;

/// <inheritdoc cref="ISongRepository"/>
public class SongRepository : RepositoryBase<Song>, ISongRepository
{
    public SongRepository(MusicAppContext context, IMapper mapper) : base(context, mapper)
    { }

    public async Task<Song?> GetByPathAsync(string musicPath, CancellationToken token = default)
    {
        return await DbSet.FirstOrDefaultAsync(m => m.Path.ToString() == musicPath);
    }

    public async Task<bool> PathExistsAsync(string musicPath, CancellationToken token = default)
    {
        return await DbSet.AsNoTracking()
            .AnyAsync(music => music.Path.ToString() == musicPath);
    }

    public async Task<IReadOnlyList<SongInfoDTO>> GetSongInfosAsync(CancellationToken token = default)
    {
        return await DbSet.AsNoTracking()
            .Include(e => e.Album)
            .OrderBy(e => e.Album.Directory.OriginalString)
            .Select(e => new SongInfoDTO
            {
                SongId = e.Id,
                AlbumId = e.AlbumId,
                AlbumName = e.Album.Name,
                SongTrackNumber = e.TrackNumber,
                SongTitle = e.Title
            })
            .OrderBy(m => m.SongTrackNumber)
            .ThenBy(m => m.SongTitle)
            .ToListAsync(token);
    }

    public async Task<IReadOnlyList<SongDetailDTO>> GetSongDetailsAsync(CancellationToken token = default)
    {
        return await DbSet.AsNoTracking()
            .Include(e => e.Album.Artist.Catalog)
            .Select(e => new SongDetailDTO
            {
                CatalogId = e.Album.Artist.CatalogId,
                ArtistId = e.Album.ArtistId,
                AlbumId = e.AlbumId,
                SongId = e.Id,
                CatalogName = e.Album.Artist.Catalog.Name,
                ArtistName = e.Album.Artist.Name,
                AlbumName = e.Album.Name,
                AlbumType = e.Album.Type,
                SongFullPath = e.Path.ToString(),
                SongTrackNumber = e.TrackNumber,
                SongTitle = e.Title,
                SongRating = e.Classification.Rating,
                SongVocalGender = e.SongDetails.VocalGender,
                SongCoveredArtist = e.SongDetails.CoveredArtist,
                SongFeaturedArtist = e.SongDetails.FeaturedArtist
            })
            .OrderBy(e => e.SongFullPath)
            .ToListAsync(token);
    }

    public async Task<IReadOnlyList<SongRankDTO>> GetAllSongRanksAsync(CancellationToken token = default)
    {
        return await DbSet.AsNoTracking()
           .Include(x => x.Album.Artist)
           .Select(x => new SongRankDTO(
               x.Id,
               x.Album.Artist.Name,
               x.Album.Name,
               x.Path.ToString(),
               x.Title,
               x.Classification.Rating))
            .ToListAsync(token);
    }

    public async Task<IReadOnlyList<SongRankDTO>> GetAllSongRankByRatingRangeAsync(int minimumRating, int maximumRating, CancellationToken token = default)
    {
        return await DbSet.AsNoTracking()
           .Include(x => x.Album.Artist)
           .Where(x => x.Classification.Rating >= minimumRating && x.Classification.Rating <= maximumRating)
           .Select(x => new SongRankDTO(
               x.Id,
               x.Album.Artist.Name,
               x.Album.Name,
               x.Path.ToString(),
               x.Title,
               x.Classification.Rating))
            .ToListAsync(token);
    }

    public async Task<bool> UpdateSongRankAsync(SongRankDTO songRank, CancellationToken token = default)
    {
        var entry = await DbSet.FindAsync(new object?[] { songRank.Id }, cancellationToken: token);

        if (entry is null)
            return false;

        entry.Classification.UpdateRating(songRank.CurrentRatingPoints);

        return await UpdateAsync(entry, token);
    }
}