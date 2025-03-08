using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Maxsys.MediaManager.MusicContext.Infra.DataEFCore.Context.Converters;

internal class SpotifyIDToStringValueConverter : ValueConverter<SpotifyID?, string?>
{
    private static string? ToDatabase(SpotifyID? model) => model?.ToString();

    private static SpotifyID? FromDatabase(string? provider) => provider is not null ? new(provider) : null;

    public SpotifyIDToStringValueConverter() : base(
        model => ToDatabase(model),
        provider => FromDatabase(provider),
        new ConverterMappingHints(size: 50, unicode: false))
    { }
}