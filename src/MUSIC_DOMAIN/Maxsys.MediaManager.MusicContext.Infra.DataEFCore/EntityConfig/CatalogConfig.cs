using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Maxsys.MediaManager.MusicContext.Infra.DataEFCore.EntityConfig;

internal class CatalogConfig : IEntityTypeConfiguration<Catalog>
{
    public void Configure(EntityTypeBuilder<Catalog> builder)
    {
        builder.ToTable("Catalog").HasKey(e => e.Id);

        // Properties
        builder.Property(e => e.Id).IsRequired();
        builder.Property(e => e.Name).HasMaxLength(50).IsRequired();

        // Indexes
        builder.HasIndex(e => e.Name).IsUnique().HasDatabaseName("AK_Catalog_Name");
    }
}