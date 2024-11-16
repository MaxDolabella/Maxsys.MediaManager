namespace Maxsys.MediaManager.MusicContext.Domain.Factories;

public static class CatalogFactory
{
    public static Catalog Create(string name)
    {
        return new Catalog(GuidGen.NewSequentialGuid(), name);
    }
}