using Maxsys.ModelCore;
using System;

namespace Maxsys.MediaManager.MusicContext.Domain.ValueObjects;

public class SongProperties : ValueObject<SongProperties>
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

    #region OVERRIDES

    protected override bool EqualsCore(SongProperties other)
    {
        return Duration == other.Duration
            && BitRate == other.BitRate;
    }

    protected override int GetHashCodeCore()
    {
        unchecked
        {
            int hashCode = Duration.GetHashCode();
            hashCode = (hashCode * 397) ^ BitRate.GetHashCode();
            return hashCode;
        }
    }

    #endregion OVERRIDES

    #region METHODS

    internal void Update(SongProperties another)
    {
        this.Duration = another.Duration;
        this.BitRate = another.BitRate;
    }

    #endregion METHODS
}