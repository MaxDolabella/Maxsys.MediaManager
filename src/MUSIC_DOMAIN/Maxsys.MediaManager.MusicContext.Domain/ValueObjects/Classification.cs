namespace Maxsys.MediaManager.MusicContext.Domain.ValueObjects;

public class Classification : IEquatable<Classification?>
{
    public const double BASE_DIF = 400.0;

    #region PROPERTIES

    public int Rating { get; protected set; }

    #endregion PROPERTIES

    #region CONSTRUCTORS

    protected Classification()
    { }

    /// <summary>
    /// Constructor for <see cref="Classification"/> class
    /// </summary>
    /// <param name="rating">is a number that represents the rating of the music. Can't be negative.
    /// <para/>Is based in 0-10 stars rating. Uses the <see cref="BASE_DIF"/> const multiplier.
    /// So a music with 5 stars shall be the following rating calculation when <paramref name="rating"/>5:
    /// <para/><c>5 x <see cref="BASE_DIF"/></c></param>
    public Classification(int rating)
    {
        Rating = rating < 0 ? 0 : rating;
    }

    /// <summary>
    /// Constructor for <see cref="Classification"/> class.
    /// <para/>Uses the <see cref="BASE_DIF"/> const multiplier to define the <see cref="Rating"/>.
    /// Assuming <see cref="BASE_DIF"/>=<c>400</c>, then a song with 5 stars rating, will have the following calculation for the <see cref="Rating"/>:
    /// <para/><see cref="Rating"/>= 400 x 5 = 2000.
    /// </summary>
    /// <param name="stars10">is a number of stars (0 from 10) that represents a classification of the music.</param>
    public Classification(byte stars10)
    {
        Rating = Stars10ToRatingPoints(stars10);
    }

    #endregion CONSTRUCTORS

    #region METHODS

    /// <summary>
    /// Updates <see cref="Rating"/> value with a new value
    /// </summary>
    /// <param name="newValue">is the <see cref="Rating"/> new value</param>
    public void UpdateRating(int newValue) => Rating = newValue;

    /// <summary>
    /// Converts the <see cref="Classification.Rating"/> value to a number between 0 and 10, based on <see cref="BASE_DIF"/>.
    /// </summary>
    /// <returns>A rating from 0 to 10.</returns>
    public byte GetStars10()
        => RatingPointsToStars10(Rating);

    /// <summary>
    /// Converts the <see cref="Classification.Rating"/> value to a number between 0 and 5, based on <see cref="BASE_DIF"/>.
    /// </summary>
    /// <returns>A rating from 0 to 5.</returns>
    public byte GetStars5()
        => RatingPointsToStars5(Rating);

    #endregion METHODS

    #region STATICS

    /// <summary>
    /// Converts an int value to a number between 0 and 10, based on <see cref="BASE_DIF"/>.
    /// </summary>
    /// <param name="ratingPoints">is a rating number</param>
    /// <returns>A rating from 0 to 10.</returns>
    public static byte RatingPointsToStars10(int ratingPoints)
    {
        var stars0_10 = Math.Round(ratingPoints / BASE_DIF, 0);
        if (stars0_10 > 10) stars0_10 = 10;
        if (stars0_10 < 0) stars0_10 = 0;

        return Convert.ToByte(stars0_10);
    }

    /// <summary>
    /// Converts an int value to a number between 0 and 5, based on <see cref="BASE_DIF"/>.
    /// </summary>
    /// <param name="ratingPoints">is a rating number</param>
    /// <returns>A rating from 0 to 5.</returns>
    public static byte RatingPointsToStars5(int ratingPoints)
    {
        var stars0_5 = Math.Round(RatingPointsToStars10(ratingPoints) / 2.0, 0);

        return Convert.ToByte(stars0_5);
    }

    /// <summary>
    /// Converts 0-5 stars in Rating
    /// </summary>
    /// <param name="stars5">a number between 0 and 5 that represents the stars classification of the music</param>
    /// <returns>an int number represents the Rating points from the stars number</returns>
    public static int Stars5ToRatingPoints(byte stars5)
    {
        var stars10 = Convert.ToByte(stars5 * 2);

        return Stars10ToRatingPoints(stars10);
    }

    /// <summary>
    /// Converts 0-10 stars in Rating
    /// </summary>
    /// <param name="stars10">a number between 0 and 10 that represents the stars classification of the music</param>
    /// <returns>an int number represents the Rating points from the stars number</returns>
    public static int Stars10ToRatingPoints(byte stars10)
    {
        var ratingPoints = (stars10 is < 0 or > 10)
            ? 0
            : Convert.ToInt32(stars10 * BASE_DIF);

        return ratingPoints;
    }

    #endregion STATICS

    #region OVERRIDES

    public override string ToString() => Rating.ToString();

    public bool Equals(Classification? other)
    {
        return other is not null
            && Rating == other.Rating;
    }

    public override bool Equals(object? obj) => Equals(obj as Classification);

    public override int GetHashCode()
    {
        return HashCode.Combine(Rating);
    }

    #endregion OVERRIDES
}