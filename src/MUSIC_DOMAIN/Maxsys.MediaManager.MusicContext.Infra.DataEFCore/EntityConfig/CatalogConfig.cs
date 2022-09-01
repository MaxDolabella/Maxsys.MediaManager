using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Maxsys.MediaManager.MusicContext.Infra.DataEFCore.EntityConfig;

internal class CatalogConfig : IEntityTypeConfiguration<Catalog>
{
    public void Configure(EntityTypeBuilder<Catalog> builder)
    {
        builder.ToTable("Catalogs")
            .HasKey(catalog => catalog.Id);

        // Properties
        builder.Property(catalog => catalog.Name)
            .HasMaxLength(50)
            .IsRequired();

        // Indexes
        builder.HasIndex(catalog => catalog.Name)
            .IsUnique().HasDatabaseName("AK_Catalogs_Name");
    }
}