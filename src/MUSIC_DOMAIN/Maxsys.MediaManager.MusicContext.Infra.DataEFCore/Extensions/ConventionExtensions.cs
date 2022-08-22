using Microsoft.EntityFrameworkCore.Metadata;

namespace ModelBuilderExtensions;

internal static class ConventionExtensions
{
    // This:
    // .Where(p => p.PropertyInfo.PropertyType == typeof(DateTime));
    // Was replaced to this:
    // .Where(p => p.ClrType == typeof(DateTime));

    /// <summary>
    /// Sets all properties of type <see cref="DateTime"/> to "date" type on the database.
    /// </summary>
    /// <param name="modelBuilder">The builder being used to construct the model for this context.
    /// Databases (and other extensions) typically define extension methods on this object that allow
    /// you to configure aspects of the model that are specific to a given database.</param>
    public static void DateTimeToDateConvention(this ModelBuilder modelBuilder)
    {
        foreach (IMutableEntityType entity in modelBuilder.Model.GetEntityTypes())
        {
            var dateTimeProps = entity.GetProperties()
                .Where(p => p.ClrType == typeof(DateTime));

            foreach (var prop in dateTimeProps)
            {
                modelBuilder.Entity(entity.Name)
                    .Property(prop.Name)
                    .HasColumnType("date");
            }
        }
    }

    /// <summary>
    /// Sets all properties of type <see cref="string"/> to "varchar" type on the database.
    /// </summary>
    /// <param name="modelBuilder">The builder being used to construct the model for this context.
    /// Databases (and other extensions) typically define extension methods on this object that allow
    /// you to configure aspects of the model that are specific to a given database.</param>
    public static void StringToVarcharConvention(this ModelBuilder modelBuilder, int lenght = 100)
    {
        foreach (IMutableEntityType entity in modelBuilder.Model.GetEntityTypes())
        {
            var dateTimeProps = entity.GetProperties()
                .Where(p => p.ClrType == typeof(string));

            foreach (var prop in dateTimeProps)
            {
                modelBuilder.Entity(entity.Name)
                    .Property(prop.Name)
                    .HasColumnType($"varchar({lenght})");
            }
        }
    }

    /// <summary>
    /// Sets all properties lengths of type <see cref="string"/> to 100 on the database.
    /// </summary>
    /// <param name="modelBuilder">The builder being used to construct the model for this context.
    /// Databases (and other extensions) typically define extension methods on this object that allow
    /// you to configure aspects of the model that are specific to a given database.</param>
    public static void StringMaxLength100Convention(this ModelBuilder modelBuilder)
    {
        foreach (IMutableEntityType entity in modelBuilder.Model.GetEntityTypes())
        {
            var stringProps = entity.GetProperties()
                .Where(p => p.ClrType == typeof(string));

            foreach (var prop in stringProps)
            {
                modelBuilder.Entity(entity.Name)
                    .Property(prop.Name)
                    .HasMaxLength(100);
            }
        }
    }
}