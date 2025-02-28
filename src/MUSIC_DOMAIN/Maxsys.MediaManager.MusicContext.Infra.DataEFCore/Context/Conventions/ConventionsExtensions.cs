using Maxsys.Core.EventSourcing;

namespace Maxsys.Core.Data.SqlServer.Conventions;

public static class ConventionsExtensions
{
    public static ModelConfigurationBuilder DateTimeToDateConvention(this ModelConfigurationBuilder configurationBuilder)
    {
        configurationBuilder.Properties<DateTime>(x => x.HaveColumnType("date"));

        return configurationBuilder;
    }

    public static ModelConfigurationBuilder StringToVarcharConvention(this ModelConfigurationBuilder configurationBuilder, int maxLength = -1)
    {
        configurationBuilder.Properties<string>(x => x.AreUnicode(false).HaveMaxLength(maxLength));

        return configurationBuilder;
    }

    public static ModelConfigurationBuilder IgnoreDomainEventConvention(this ModelConfigurationBuilder configurationBuilder, int maxLength = -1)
    {
        configurationBuilder.IgnoreAny<DomainEvent>();

        return configurationBuilder;
    }
}