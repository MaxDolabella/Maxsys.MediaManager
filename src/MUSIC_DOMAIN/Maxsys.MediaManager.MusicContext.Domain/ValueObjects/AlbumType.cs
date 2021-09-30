using System.ComponentModel;

namespace Maxsys.MediaManager.MusicContext.Domain.ValueObjects
{
    public enum AlbumType : byte
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
}