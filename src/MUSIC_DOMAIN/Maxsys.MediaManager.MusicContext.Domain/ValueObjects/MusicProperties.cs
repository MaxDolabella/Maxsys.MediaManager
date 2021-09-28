using System;
using Maxsys.ModelCore;

namespace Maxsys.MediaManager.MusicContext.Domain.ValueObjects
{
    public class MusicProperties : ValueObject<MusicProperties>
    {
        #region PROPERTIES
        public TimeSpan Duration { get; protected set; }
        public int BitRate { get; protected set; }
        #endregion

        #region CONSTRUCTORS
        protected MusicProperties() { }

        internal MusicProperties(TimeSpan duration, int bitRate)
        {
            Duration = duration;
            BitRate = bitRate;
        }
        #endregion

        #region OVERRIDES
        protected override bool EqualsCore(MusicProperties other)
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
        #endregion

        #region METHODS
        
        internal void UpdateFrom(MusicProperties another)
        {
            this.Duration = another.Duration;
            this.BitRate = another.BitRate;
        }
        #endregion
    }
}
