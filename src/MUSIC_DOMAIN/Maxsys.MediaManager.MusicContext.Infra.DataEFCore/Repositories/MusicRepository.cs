using Maxsys.MediaManager.MusicContext.Domain.DTO;
using Maxsys.MediaManager.MusicContext.Domain.Entities;
using Maxsys.MediaManager.MusicContext.Domain.Interfaces.Repositories;
using Maxsys.MediaManager.MusicContext.Infra.DataEFCore.Context;
using Maxsys.MediaManager.MusicContext.Infra.DataEFCore.Repositories.Common;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Maxsys.MediaManager.MusicContext.Infra.DataEFCore.Repositories
{
    /// <inheritdoc cref="IMusicRepository"/>
    public class MusicRepository : RepositoryBase<Music>, IMusicRepository
    {
        public MusicRepository(MusicAppContext context) : base(context)
        { }

        public async Task<Music> GetByPathAsync(string musicPath)
        {
            return await DbSet.FirstOrDefaultAsync(m => m.FullPath == musicPath);
        }

        public async Task<bool> PathExistsAsync(string musicPath)
        {
            return await DbSet.AsNoTracking()
                .AnyAsync(music => music.FullPath == musicPath);
        }

        public async Task<IReadOnlyList<MusicInfoDTO>> GetMusicsAsync()
        {
            return await ReadOnlySet
                .Include(e => e.Album)
                .OrderBy(e => e.Album.AlbumDirectory)
                .Select(e => new MusicInfoDTO
                {
                    AlbumId = e.AlbumId,
                    AlbumName = e.Album.Name,

                    MusicId = e.Id,
                    MusicTrack = e.TrackNumber,
                    MusicTitle = e.Title,
                })
                .OrderBy(m => m.MusicTrack)
                .ThenBy(m => m.MusicTitle)
                .ToListAsync();
        }

        public async Task<IReadOnlyList<MusicListDTO>> GetMusicListAsync()
        {
            return await ReadOnlySet
                .Include(e => e.Album.Artist.MusicCatalog)
                .Select(e => new MusicListDTO
                {
                    MusicCatalogName = e.Album.Artist.MusicCatalog.Name,
                    ArtistName = e.Album.Artist.Name,
                    AlbumName = e.Album.Name,
                    AlbumType = e.Album.AlbumType,
                    MusicId = e.Id,
                    MusicFullPath = e.FullPath,
                    MusicTrackNumber = e.TrackNumber,
                    MusicTitle = e.Title,
                    MusicCoveredArtist = e.MusicDetails.CoveredArtist,
                    MusicFeaturedArtist = e.MusicDetails.FeaturedArtist,
                    MusicVocalGender = e.MusicDetails.VocalGender,
                    MusicRating = e.Classification.Rating
                })
                .OrderBy(e => e.MusicFullPath)
                .ToListAsync();
        }
    }
}