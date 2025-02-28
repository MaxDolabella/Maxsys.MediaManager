namespace Maxsys.MediaManager.MusicContext.Domain.Factories;

public static class PlaylistFactory
{
    public static Playlist Create(Guid id, string name, string? spotifyID) 
        => new(id, name, spotifyID);
    
    public static Playlist Create(string name, string? spotifyID) 
        => Create(GuidGen.NewSequentialGuid(), name, spotifyID);
}