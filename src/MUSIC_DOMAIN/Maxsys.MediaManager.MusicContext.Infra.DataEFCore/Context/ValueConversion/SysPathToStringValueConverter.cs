using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Maxsys.Core.Data.ValueConversion;

internal class SysPathToStringValueConverter : ValueConverter<SysPath?, string?>
{
    private static string? ToDatabase(SysPath? model) => model?.ToString();

    private static SysPath? FromDatabase(string? provider) => provider is not null ? new(provider) : null;

    public SysPathToStringValueConverter() : base(
        model => ToDatabase(model),
        provider => FromDatabase(provider),
        new ConverterMappingHints(size: 255, unicode: false))
    { }
}