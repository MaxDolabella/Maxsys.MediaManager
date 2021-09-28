namespace Maxsys.MediaManager.MusicContext.Domain.ValueObjects
{
    public enum AlbumType : byte
    {
        /// <summary>
        /// Undefined Albuns<para/>Most used for musics without albums. "Misc - Iron Maiden"
        /// </summary>
        Undefined = 0,

        /// <summary>
        /// Studio Albuns<para/>"Brave New World"
        /// </summary>
        Studio,

        /// <summary>
        /// Live Albuns<para/>"Rock In Rio"
        /// </summary>
        Live,

        /// <summary>
        /// Compilations Albuns<para/>"The Best of" Albuns
        /// </summary>
        Compilation,

        /// <summary>
        /// Bootlegs Albuns<para/>Amateur records
        /// </summary>
        Bootleg,

        /// <summary>
        /// Various Artists Albuns<para/>"Female Metal"
        /// </summary>
        Various = 9,

        /// <summary>
        /// Other Types Albuns<para/>Other Types
        /// </summary>
        Others = 10

    }
}
