namespace Maxsys.MediaManager.MusicContext.Domain.Options
{
    public class MusicSettings
    {
        public const string Section = "MusicSettings";

        /// <summary>
        /// Defines the Music library folder.
        /// <br/>
        /// Example: <code>D:\Music\</code>
        /// </summary>
        public string MusicLibraryFolder { get; set; }
    }
}