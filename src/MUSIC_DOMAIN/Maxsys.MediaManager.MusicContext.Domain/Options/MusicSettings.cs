namespace Maxsys.MediaManager.MusicContext.Domain.Options;

public class MusicSettings
{
    public const string SECTION = nameof(MusicSettings);

    /// <summary>
    /// Defines the Song library folder.
    /// <br/>
    /// Example: <code>D:\Music\</code>
    /// </summary>
    public string MusicLibraryFolder { get; set; }

    /// <summary>
    /// Defines the name of playlists folder.
    /// <br/>
    /// Example: <code>Playlist</code>
    /// Then playlist full path: <code>D:\Music\Playlist\</code>
    /// </summary>
    public string PlaylistFolderName { get; set; }
}