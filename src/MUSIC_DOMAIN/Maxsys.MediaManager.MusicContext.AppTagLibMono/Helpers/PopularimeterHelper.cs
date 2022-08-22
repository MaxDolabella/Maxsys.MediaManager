using Maxsys.MediaManager.MusicContext.Domain.ValueObjects;
using TagLib.Id3v2;

namespace Maxsys.MediaManager.MusicContext.AppTagLibMono.Helpers;

public static class PopularimeterHelper
{
    private const string DEFAULT_POPM_USER = "Windows Media Player 9 Series"; // "no@email";

    private static readonly byte[] POPM_RATINGS =
    {
    //   VAL   |  index  |  0-10  |
    //=========|=========|========|
          0, //|    0    |     0  |
          2, //|    1    |   0.5  |
          1, //|    2    |     1  |
         32, //|    3    |   1.5  |
         64, //|    4    |     2  |
         96, //|    5    |   2.5  |
        128, //|    6    |     3  |
        160, //|    7    |   3.5  |
        196, //|    8    |     4  |
        224, //|    9    |   4.5  |
        255  //|   10    |     5  |
    };

    private static byte PopularimeterRatingToStars10(byte popmValue)
    {
        return popmValue switch
        {
            0 => 0,
            1 => 2,
            <= 22 => 1,
            <= 31 => 2,
            <= 63 => 3,
            <= 95 => 4,
            <= 127 => 5,
            <= 159 => 6,
            <= 195 => 7,
            <= 223 => 8,
            <= 254 => 9,
            255 => 10
        };
    }

    private static PopularimeterFrame GetPopularimeterFrame(Tag tags)
    {
        return PopularimeterFrame.Get(tags, DEFAULT_POPM_USER, true);
    }

    /// <summary>
    /// Gets a 0-10 rating from tags representing the rating of music
    /// </summary>
    /// <param name="tags"></param>
    /// <returns>a number between 0 and 10 </returns>
    public static byte GetStars10FromTags(Tag tags)
    {
        var popm = GetPopularimeterFrame(tags);

        return PopularimeterRatingToStars10(popm.Rating);
    }

    /// <summary>
    /// Converts a 0-10 rating into <see cref="PopularimeterFrame.Rating">PopularimeterFrame Rating</see> (0-255).
    /// </summary>
    /// <param name="stars10">a number from 0 to 10</param>
    /// <returns></returns>
    public static byte Stars10ToPopularimeterRating(byte stars10)
    {
        return stars10 > 10
            ? throw new System.ArgumentOutOfRangeException($"Value of '{nameof(stars10)}'[{stars10}] must be between 0 and 10")
            : POPM_RATINGS[stars10];
    }

    public static void WriteFromClassification(Tag tags, Classification classification)
    {
        var stars10 = classification.GetStars10();

        WriteFromStars10(tags, stars10);
    }

    public static void WriteFromStars10(Tag tags, byte stars10)
    {
        var popm = GetPopularimeterFrame(tags);

        popm.Rating = Stars10ToPopularimeterRating(stars10);
    }

    /*
    public static PopularimeterFrame[] GetAllSongRanksAsync(Tag tag)
    {
        var popmFrames = new PopularimeterFrame[] { };

        PopularimeterFrame popm;
        foreach (Frame frame in tag)
        {
            popm = frame as PopularimeterFrame;

            if (!(popm is null))
                popmFrames.Append(popm);
        }

        return popmFrames;
    }
    */
}