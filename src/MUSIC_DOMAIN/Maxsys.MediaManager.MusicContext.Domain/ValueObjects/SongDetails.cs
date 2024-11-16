using Maxsys.MediaManager.MusicContext.Domain.Enums;

namespace Maxsys.MediaManager.MusicContext.Domain.ValueObjects;

public class SongDetails : IEquatable<SongDetails?>
{
    #region PROPERTIES

    public bool IsBonusTrack { get; protected set; }
    public VocalGenders VocalGender { get; protected set; }
    public string? CoveredArtist { get; protected set; }
    public string? FeaturedArtist { get; protected set; }

    #endregion PROPERTIES

    #region CONSTRUCTORS

    protected SongDetails()
    { }

    internal SongDetails(bool isBonusTrack, VocalGenders vocalGender, string? coveredArtist, string? featuredArtist)
    {
        IsBonusTrack = isBonusTrack;
        VocalGender = vocalGender;
        CoveredArtist = coveredArtist;
        FeaturedArtist = featuredArtist;
    }

    #endregion CONSTRUCTORS

    #region OVERRIDES

    public bool Equals(SongDetails? other)
    {
        return other is not null
            && IsBonusTrack == other.IsBonusTrack
            && VocalGender == other.VocalGender
            && CoveredArtist == other.CoveredArtist
            && FeaturedArtist == other.FeaturedArtist;
    }

    public override bool Equals(object? obj) => Equals(obj as SongDetails);

    public override int GetHashCode()
    {
        return HashCode.Combine(IsBonusTrack, VocalGender, CoveredArtist, FeaturedArtist);
    }

    #endregion OVERRIDES
}