using System.ComponentModel;

namespace Maxsys.MediaManager.MusicContext.Domain.Enums;

/// <summary>
/// <list type="bullet">
/// <item>
///     <term>0.<see cref="NotDefined"/></term>
///     <description>NotDefined.</description>
/// </item>
/// <item>
///     <term>1.<see cref="Male"/></term>
///     <description>Male.</description>
/// </item>
/// <item>
///     <term>2.<see cref="Female"/></term>
///     <description>Female.</description>
/// </item>
/// <item>
///     <term>3.<see cref="Mixed"/></term>
///     <description>Mixed.</description>
/// </item>
/// </list>
/// </summary>
public enum VocalGenders : byte
{
    /// <summary>
    /// NotDefined
    /// </summary>
    [Description("Not Defined")]
    NotDefined = 0,

    /// <summary>
    /// Male
    /// </summary>
    [Description("Male")]
    Male = 1,

    /// <summary>
    /// Female
    /// </summary>
    [Description("Female")]
    Female = 2,

    /// <summary>
    /// Mixed
    /// </summary>
    [Description("Mixed")]
    Mixed = 3
}