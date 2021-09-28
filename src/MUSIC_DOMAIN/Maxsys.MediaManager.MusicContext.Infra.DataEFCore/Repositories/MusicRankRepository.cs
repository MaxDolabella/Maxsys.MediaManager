using FluentValidation.Results;
using Maxsys.MediaManager.MusicContext.Domain.Entities;
using Maxsys.MediaManager.MusicContext.Domain.Interfaces.Repositories;
using Maxsys.MediaManager.MusicContext.Infra.DataEFCore.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Maxsys.MediaManager.MusicContext.Infra.DataEFCore.Repositories
{
    public class MusicRankRepository : IMusicRankRepository
    {
        #region FIELDS
        protected DbSet<Music> DbSet;
        protected readonly DbContext Context;
        #endregion
        #region CONSTRUCTOR
        public MusicRankRepository(MusicAppContext dbContext)
        {
            Context = dbContext;
            DbSet = Context.Set<Music>();
        }
        #endregion

        public ICollection<MusicRank> GetAll()
        {
            var query = DbSet
                .Include(x => x.Album.Artist)
                .Select(x => new { x.Id, x.FullPath, x.Title, ArtistName = x.Album.Artist.Name, AlbumName = x.Album.Name, RatingPoints = x.Classification.Rating });
            
            return query
                .Select(a => new MusicRank(a.Id, a.FullPath, a.Title, a.ArtistName, a.AlbumName, a.RatingPoints))
                .ToList();
        }
        public ICollection<MusicRank> GetAllByRating(int minimumRating, int maximumRating)
        {
            var query = DbSet
                .Where(x => x.Classification.Rating >= minimumRating && x.Classification.Rating <= maximumRating)
                .Include(x => x.Album.Artist)
                .Select(x => new { x.Id, x.FullPath, x.Title, ArtistName = x.Album.Artist.Name, AlbumName = x.Album.Name, RatingPoints = x.Classification.Rating });

            return query
                .Select(a => new MusicRank(a.Id, a.FullPath, a.Title, a.ArtistName, a.AlbumName, a.RatingPoints))
                .ToList();
        }
        public ValidationResult Update(MusicRank obj)
        {
            var entry = DbSet.Find(obj.Id);
            
            entry.UpdateRatingPoints(obj.RatingPoints);

            var result = new ValidationResult();
            try
            {
                Context.SaveChanges();
            }
            catch (Exception ex)
            {
                result.AddFailure("MusicRankRepository.Update(MusicRank)", ex.Message);
            }

            return result;
        }

        #region DIPOSABLE IMPLEMENTATION
        public void Dispose()
        {
            //Context.Dispose();
            GC.SuppressFinalize(this);
        }
        #endregion DIPOSABLE IMPLEMENTATION
    }
}
