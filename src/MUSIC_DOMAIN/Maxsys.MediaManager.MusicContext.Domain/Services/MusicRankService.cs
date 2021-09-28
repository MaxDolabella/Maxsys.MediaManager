using FluentValidation.Results;
using Maxsys.MediaManager.MusicContext.Domain.Entities;
using Maxsys.MediaManager.MusicContext.Domain.Interfaces.Mp3;
using Maxsys.MediaManager.MusicContext.Domain.Interfaces.Repositories;
using Maxsys.MediaManager.MusicContext.Domain.Interfaces.Services;
using System.Collections.Generic;

namespace Maxsys.MediaManager.MusicContext.Domain.Services
{
    public class MusicRankService : IMusicRankService
    {
        private readonly IMusicRankRepository _repository;
        private readonly ITagService _tagService;

        #region CONTRUCTORS

        public MusicRankService(IMusicRankRepository repository, ITagService tagService)
        {
            _repository = repository;
            _tagService = tagService;
        }

        #endregion CONTRUCTORS

        public ICollection<MusicRank> GetAll() => _repository.GetAll();

        public ICollection<MusicRank> GetAllByRating(int minimumRating, int maximumRating)
            => _repository.GetAllByRating(minimumRating, maximumRating);

        /// <inheritdoc/>
        public ValidationResult UpdateRating(MusicRank music)
        {
            ValidationResult result = null;
            if (music.RatingHasChanged())
            {
                result = _repository.Update(music);
                if (result.IsValid && music.Stars10HasChanged())
                {
                    result = _tagService.WriteRating(music.FullPath, music.GetStars10());
                }
            }

            return result ?? new ValidationResult();
        }
    }
}