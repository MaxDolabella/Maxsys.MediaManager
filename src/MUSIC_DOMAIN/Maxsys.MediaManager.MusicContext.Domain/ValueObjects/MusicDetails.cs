using Maxsys.ModelCore;

namespace Maxsys.MediaManager.MusicContext.Domain.ValueObjects
{
    public class MusicDetails : ValueObject<MusicDetails>
    {
        #region PROPERTIES
        public bool IsBonusTrack { get; protected set; }
        public VocalGender VocalGender { get; protected set; }
        public string CoveredArtist { get; protected set; }
        public string FeaturedArtist { get; protected set; }
        #endregion

        #region CONSTRUCTORS
        protected MusicDetails() { }

        internal MusicDetails(bool isBonusTrack, VocalGender vocalGender, string coveredArtist, string featuredArtist)
        {
            IsBonusTrack = isBonusTrack;
            VocalGender = vocalGender;
            CoveredArtist = coveredArtist;
            FeaturedArtist = featuredArtist;
        }
        #endregion

        #region OVERRIDES
        protected override bool EqualsCore(MusicDetails other)
        {
            return IsBonusTrack == other.IsBonusTrack
                && VocalGender == other.VocalGender
                && CoveredArtist == other.CoveredArtist
                && FeaturedArtist == other.FeaturedArtist;
        }

        protected override int GetHashCodeCore()
        {
            unchecked
            {
                int hashCode = IsBonusTrack.GetHashCode();
                hashCode = (hashCode * 397) ^ VocalGender.GetHashCode();
                hashCode = (hashCode * 397) ^ CoveredArtist.GetHashCode();
                hashCode = (hashCode * 397) ^ FeaturedArtist.GetHashCode();
                return hashCode;
            }
        }
        #endregion
    }
}
