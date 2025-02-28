namespace Maxsys.MediaManager.MusicContext.Domain.Factories;

public static class CatalogFactory
{
    public static Catalog Create(Guid id, string name) 
        => new(id, name);

    public static Catalog Create(string name) 
        => Create(GuidGen.NewSequentialGuid(), name);
}