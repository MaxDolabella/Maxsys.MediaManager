namespace Maxsys.MediaManager.MusicContext.Domain.Factories;

public static class ArtistFactory
{
    public static Artist Create(Guid id, Guid musicCatalogId, string name, string? spotifyID)
        => new(id, musicCatalogId, name, spotifyID);

    public static Artist Create(Guid musicCatalogId, string name, string? spotifyID)
        => Create(GuidGen.NewSequentialGuid(), musicCatalogId, name, spotifyID);
}