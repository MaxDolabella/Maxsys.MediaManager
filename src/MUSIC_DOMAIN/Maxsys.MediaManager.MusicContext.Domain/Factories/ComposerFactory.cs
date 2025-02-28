namespace Maxsys.MediaManager.MusicContext.Domain.Factories;

public static class ComposerFactory
{
    public static Composer Create(Guid id, string name)
        => new(id, name);

    public static Composer Create(string name)
        => Create(GuidGen.NewSequentialGuid(), name);
}