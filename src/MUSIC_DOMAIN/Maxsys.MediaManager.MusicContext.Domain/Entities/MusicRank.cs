using Maxsys.MediaManager.MusicContext.Domain.ValueObjects;
using System;

namespace Maxsys.MediaManager.MusicContext.Domain.Entities
{
    public class MusicRank
    {
        public Guid Id { get; }
        public string FullPath { get;  }
        public string Title { get;  }
        public string ArtistName { get; }
        public string AlbumName { get; }
        public int InitialRatingPoints { get; }
        public int RatingPoints { get; private set; }

        public MusicRank(Guid id, string fullPath, string title, string artistName, string albumName, int ratingPoints)
        {
            (Id, FullPath, Title, ArtistName, AlbumName, RatingPoints) 
                = (id, fullPath, title, artistName, albumName, ratingPoints);

            InitialRatingPoints = ratingPoints;
        }

        public void UpdateRatingPoints(int newValue) => RatingPoints = newValue;

        public bool RatingHasChanged() => RatingPoints != InitialRatingPoints;
        
        public bool Stars10HasChanged() => Classification.RatingPointsToStars10(RatingPoints) 
            != Classification.RatingPointsToStars10(InitialRatingPoints);

        public byte GetStars10() => Classification.RatingPointsToStars10(RatingPoints);

    }
}
