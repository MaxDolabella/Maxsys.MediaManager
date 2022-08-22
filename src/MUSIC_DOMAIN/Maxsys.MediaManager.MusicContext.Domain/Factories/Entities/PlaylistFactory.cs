namespace Maxsys.MediaManager.MusicContext.Domain.Factories;

public static class PlaylistFactory
{
    public static Playlist Create(string name)
    {
        return new Playlist(GuidGen.NewSequentialGuid(), name);
    }
}