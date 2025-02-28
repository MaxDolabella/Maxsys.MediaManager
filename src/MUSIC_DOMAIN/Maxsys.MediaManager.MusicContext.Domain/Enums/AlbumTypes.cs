using System.ComponentModel;

namespace Maxsys.MediaManager.MusicContext.Domain.Enums;


/// <summary>
/// <list type="bullet">
/// <item>
///     <term>0.<see cref="Undefined"/></term>
///     <description>Undefined Albuns<para/>Most used for musics without albums. "Misc - Iron Maiden".</description>
/// </item>
/// <item>
///     <term>1.<see cref="Studio"/></term>
///     <description>Studio Albuns<para/>"Brave New World".</description>
/// </item>
/// <item>
///     <term>2.<see cref="Live"/></term>
///     <description>Live Albuns<para/>"Rock In Rio".</description>
/// </item>
/// <item>
///     <term>3.<see cref="Compilation"/></term>
///     <description>Compilations Albuns<para/>"The Best of" Albuns.</description>
/// </item>
/// <item>
///     <term>4.<see cref="Bootleg"/></term>
///     <description>Compilations Albuns<para/>"The Best of" Albuns.</description>
/// </item>
/// <item>
///     <term>9.<see cref="Various"/></term>
///     <description>Various Artists Albuns<para/>"Female Metal".</description>
/// </item>
/// <item>
///     <term>10.<see cref="Others"/></term>
///     <description>Other Types Albuns<para/>Other Types.</description>
/// </item>
/// </list>
/// </summary>
public enum AlbumTypes : byte
{
    /// <summary>
    /// Undefined Albuns<para/>Most used for musics without albums. "Misc - Iron Maiden"
    /// </summary>
    [Description("Undefined album type")]
    Undefined = 0,

    /// <summary>
    /// Studio Albuns<para/>"Brave New World"
    /// </summary>
    [Description("Studio album")]
    Studio,

    /// <summary>
    /// Live Albuns<para/>"Rock In Rio"
    /// </summary>
    [Description("Live album")]
    Live,

    /// <summary>
    /// Compilations Albuns<para/>"The Best of" Albuns
    /// </summary>
    [Description("Compilation album")]
    Compilation,

    /// <summary>
    /// Bootlegs Albuns<para/>Amateur records
    /// </summary>
    [Description("Amateur record")]
    Bootleg,

    /// <summary>
    /// Various Artists Albuns<para/>"Female Metal"
    /// </summary>
    [Description("Various artists album")]
    Various = 9,

    /// <summary>
    /// Other Types Albuns<para/>Other Types
    /// </summary>
    [Description("Other type")]
    Others = 10
}