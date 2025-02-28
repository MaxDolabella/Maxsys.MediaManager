using System.Reflection;

namespace Maxsys.MediaManager.DatabaseInitializer.Extensions;

internal static class EmbeddedResourceHelper
{
    public static string? GetSeedJson(this Assembly assembly, string name)
    {
        string text;

        using (var stream = assembly.GetManifestResourceStream($"Maxsys.MediaManager.DatabaseInitializer.Seed.{name}.json"))
        {
            if (stream == null)
            {
                return null;
            }

            using (var reader = new StreamReader(stream))
            {
                text = reader.ReadToEnd();
            }
        }

        return text;
    }
}