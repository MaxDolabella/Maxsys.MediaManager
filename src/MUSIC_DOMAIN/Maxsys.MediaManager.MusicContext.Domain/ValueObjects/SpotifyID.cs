using System.Diagnostics;

namespace Maxsys.MediaManager.MusicContext.Domain.ValueObjects;

[DebuggerDisplay("{ToString(),nq}")]
public struct SpotifyID
{
    private readonly string _value;

    public SpotifyID(string value)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(value, nameof(value));

        _value = value;
    }

    public override readonly string ToString() => _value;

    #region Operators

    public static implicit operator SpotifyID?(string? value) => value is null ? null : new SpotifyID(value);

    public static implicit operator string?(SpotifyID? value) => value?.ToString();

    public static bool operator ==(SpotifyID? a, SpotifyID? b) => a?._value == b?._value;

    public static bool operator !=(SpotifyID? a, SpotifyID? b) => !(a == b);

    public static bool operator ==(SpotifyID? a, string? b) => a?._value == b;

    public static bool operator !=(SpotifyID? a, string? b) => !(a == b);

    public static bool operator ==(string? a, SpotifyID? b) => a == b?._value;

    public static bool operator !=(string? a, SpotifyID? b) => !(a == b);

    #endregion Operators
}