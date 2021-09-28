using FluentValidation.Results;
using Maxsys.MediaManager.MusicContext.Domain.Entities;
using System.Collections.Generic;

namespace Maxsys.MediaManager.MusicContext.Domain.Interfaces.Services
{
    public interface IMusicRankService
    {
        ICollection<MusicRank> GetAll();
        ICollection<MusicRank> GetAllByRating(int minimumRating, int maximumRating);
        ValidationResult UpdateRating(MusicRank music);
    }
}