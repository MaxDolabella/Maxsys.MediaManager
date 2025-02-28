namespace Maxsys.MediaManager.MusicContext.Domain.ValueObjects;

public class SongProperties : IEquatable<SongProperties?>
{
    #region PROPERTIES

    public TimeSpan Duration { get; protected set; }
    public int BitRate { get; protected set; }

    #endregion PROPERTIES

    #region CONSTRUCTORS

    protected SongProperties()
    { }

    internal SongProperties(TimeSpan duration, int bitRate)
    {
        Duration = duration;
        BitRate = bitRate;
    }

    #endregion CONSTRUCTORS

    #region METHODS

    internal void Update(TimeSpan duration, int bitRate)
    {
        Duration = duration;
        BitRate = bitRate;
    }

    #endregion METHODS

    #region OVERRIDES

    public bool Equals(SongProperties? other)
    {
        return other is not null
            && Duration == other.Duration
            && BitRate == other.BitRate;
    }

    public override bool Equals(object? obj) => Equals(obj as SongProperties);

    public override int GetHashCode()
    {
        return HashCode.Combine(Duration, BitRate);
    }

    #endregion OVERRIDES
}