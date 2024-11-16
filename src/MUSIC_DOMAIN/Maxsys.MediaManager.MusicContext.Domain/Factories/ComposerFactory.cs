namespace Maxsys.MediaManager.MusicContext.Domain.Factories;

public static class ComposerFactory
{
    public static Composer Create(string name)
    {
        return new Composer(GuidGen.NewSequentialGuid(), name);
    }
}