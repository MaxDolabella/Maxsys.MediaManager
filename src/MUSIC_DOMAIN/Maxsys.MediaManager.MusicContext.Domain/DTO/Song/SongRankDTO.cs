namespace Maxsys.MediaManager.MusicContext.Domain.DTO;

public record struct SongRankDTO
{
    public Guid Id { get; init; }
    public Uri FullPath { get; init; }
    public string Title { get; init; }
    public string Artist { get; init; }
    public string Album { get; init; }
    public int InitialRatingPoints { get; init; }
    public int CurrentRatingPoints { get; private set; }

    public SongRankDTO(Guid id, string artistName, string albumName, Uri fullPath, string title, int ratingPoints)
    {
        (Id, FullPath, Title, Artist, Album, InitialRatingPoints) =
        (id, fullPath, title, artistName, albumName, ratingPoints);

        CurrentRatingPoints = InitialRatingPoints;
    }

    public void UpdateRatingPoints(int newValue) => CurrentRatingPoints = newValue;

    public bool RatingHasChanged() => CurrentRatingPoints != InitialRatingPoints;

    public bool Stars10HasChanged() => Classification.RatingPointsToStars10(CurrentRatingPoints)
        != Classification.RatingPointsToStars10(InitialRatingPoints);

    public byte GetStars10() => Classification.RatingPointsToStars10(CurrentRatingPoints);
}