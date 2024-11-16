namespace Maxsys.MediaManager.MusicContext.Domain.Factories;

public static class ArtistFactory
{
    public static Artist Create(Guid musicCatalogId, string name)
    {
        return new Artist(GuidGen.NewSequentialGuid(), musicCatalogId, name);
    }
}