using Maxsys.MediaManager.CoreDomain_;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Maxsys.MediaManager.MusicContext.Infra.DataEFCore.Context.Converters;

internal class SystemPathToStringValueConverter : ValueConverter<SystemPath?, string?>
{
    private static string? ToDatabase(SystemPath? model) => model?.ToString();

    private static SystemPath? FromDatabase(string? provider) => provider is not null ? new(provider) : null;

    public SystemPathToStringValueConverter() : base(
        model => ToDatabase(model),
        provider => FromDatabase(provider),
        new ConverterMappingHints(size: 255, unicode: false))
    { }
}