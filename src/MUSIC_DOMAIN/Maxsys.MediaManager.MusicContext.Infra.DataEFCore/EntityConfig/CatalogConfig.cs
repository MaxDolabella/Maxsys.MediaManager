using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Maxsys.MediaManager.MusicContext.Infra.DataEFCore.EntityConfig;

internal class CatalogConfig : IEntityTypeConfiguration<Catalog>
{
    public void Configure(EntityTypeBuilder<Catalog> builder)
    {
        builder.ToTable("Catalogs")
            .HasKey(musicCatalog => musicCatalog.Id);

        // Properties
        builder.Property(musicCatalog => musicCatalog.Name)
            .HasMaxLength(50)
            .IsRequired();

        // Indexes
        builder.HasIndex(musicCatalog => musicCatalog.Name)
            .IsUnique().HasDatabaseName("AK_Catalogs_Name");
    }
}