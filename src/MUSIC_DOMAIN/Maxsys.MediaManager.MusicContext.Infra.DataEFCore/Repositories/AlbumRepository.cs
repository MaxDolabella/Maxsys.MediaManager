using Maxsys.MediaManager.MusicContext.Domain.DTO;
using Maxsys.MediaManager.MusicContext.Domain.Entities;
using Maxsys.MediaManager.MusicContext.Domain.Interfaces.Repositories;
using Maxsys.MediaManager.MusicContext.Infra.DataEFCore.Context;
using Maxsys.MediaManager.MusicContext.Infra.DataEFCore.Repositories.Common;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Maxsys.MediaManager.MusicContext.Infra.DataEFCore.Repositories
{
    /// <summary>
    ///
    /// </summary>
    public class AlbumRepository : RepositoryBase<Album>, IAlbumRepository
    {
        public AlbumRepository(MusicAppContext dbContext) : base(dbContext)
        { }

        public async Task<IReadOnlyList<string>> GetGenresAsync()
        {
            return await ReadOnlySet
                .Select(entity => entity.Genre)
                .Distinct()
                .OrderBy(genre => genre)
                .ToListAsync();
        }

        public async Task<IReadOnlyList<MusicTrackAndTitleDTO>> GetTracksAndTitlesAsync(Guid albumId)
        {
            return await ReadOnlySet
                .Include(a => a.Musics)
                .Where(a => a.Id == albumId)
                .SelectMany(a => a.Musics)
                .Select(m => new MusicTrackAndTitleDTO
                {
                    Track = m.TrackNumber,
                    Title = m.Title
                })
                .OrderBy(m => m.Track)
                .ThenBy(m => m.Title)
                .ToListAsync();
        }

        public async Task<AlbumTagDTO> GetAlbumTagDTO(Guid albumId)
        {
            return await ReadOnlySet
                .Include(a => a.Artist)
                .Where(a => a.Id == albumId)
                .Select(a => new AlbumTagDTO
                {
                    ArtistName = a.Artist.Name,
                    AlbumName = a.Name,
                    AlbumYear = a.Year,
                    AlbumGenre = a.Genre,
                    AlbumCover = a.AlbumCover
                }).FirstOrDefaultAsync();
        }

        public async Task<IReadOnlyList<AlbumInfoDTO>> GetAlbumInfosAsync()
        {
            return await ReadOnlySet
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
                .ToListAsync();
        }

        public async Task<IReadOnlyList<AlbumListDTO>> GetAlbumListsAsync()
        {
            return await ReadOnlySet
                .Include(e => e.Artist.MusicCatalog)
                .Include(e => e.Musics)
                .Select(e => new AlbumListDTO
                {
                    AlbumId = e.Id,
                    AlbumMusicCatalogName = e.Artist.MusicCatalog.Name,
                    AlbumArtistName = e.Artist.Name,
                    AlbumName = e.Name,
                    AlbumType = e.AlbumType,
                    AlbumYear = e.Year,
                    AlbumDirectory = e.AlbumDirectory,
                    AlbumMusicCount = e.Musics.Count
                })
                .OrderBy(m => m.AlbumDirectory)
                .ToListAsync();
        }

        public async Task<int> MusicsCountAsync(Guid albumId)
        {
            return await ReadOnlySet
                .Include(e => e.Musics)
                .Where(e => e.Id == albumId)
                .Select(e => e.Musics.Count)
                .FirstOrDefaultAsync();
        }
    }
}