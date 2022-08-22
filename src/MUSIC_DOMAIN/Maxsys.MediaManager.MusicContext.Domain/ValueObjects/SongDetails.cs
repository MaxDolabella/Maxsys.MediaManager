using Maxsys.ModelCore;
using System;

namespace Maxsys.MediaManager.MusicContext.Domain.ValueObjects;

public class SongDetails : ValueObject<SongDetails>
{
    #region PROPERTIES

    public bool IsBonusTrack { get; protected set; }
    public VocalGender VocalGender { get; protected set; }
    public string? CoveredArtist { get; protected set; }
    public string? FeaturedArtist { get; protected set; }

    #endregion PROPERTIES

    #region CONSTRUCTORS

    protected SongDetails()
    { }

    internal SongDetails(bool isBonusTrack, VocalGender vocalGender, string? coveredArtist, string? featuredArtist)
    {
        IsBonusTrack = isBonusTrack;
        VocalGender = vocalGender;
        CoveredArtist = coveredArtist;
        FeaturedArtist = featuredArtist;
    }

    #endregion CONSTRUCTORS

    #region OVERRIDES

    protected override bool EqualsCore(SongDetails other)
    {
        return IsBonusTrack == other.IsBonusTrack
            && VocalGender == other.VocalGender
            && CoveredArtist == other.CoveredArtist
            && FeaturedArtist == other.FeaturedArtist;
    }

    protected override int GetHashCodeCore()
    {
        return HashCode.Combine(IsBonusTrack, VocalGender, CoveredArtist, FeaturedArtist);
    }

    #endregion OVERRIDES
}