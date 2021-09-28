using FluentValidation.Results;
using Maxsys.MediaManager.MusicContext.Domain.Entities;
using System;
using System.Collections.Generic;

namespace Maxsys.MediaManager.MusicContext.Domain.Interfaces.Repositories
{
    public interface IMusicRankRepository : IDisposable
    {
        ICollection<MusicRank> GetAll();
        ICollection<MusicRank> GetAllByRating(int minimumRating, int maximumRating);
        ValidationResult Update(MusicRank obj);
    }
}