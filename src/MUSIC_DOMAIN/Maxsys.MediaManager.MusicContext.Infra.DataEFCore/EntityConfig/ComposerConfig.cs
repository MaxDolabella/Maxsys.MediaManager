using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Maxsys.MediaManager.MusicContext.Infra.DataEFCore.EntityConfig;

internal class ComposerConfig : IEntityTypeConfiguration<Composer>
{
    public void Configure(EntityTypeBuilder<Composer> builder)
    {
        builder.ToTable("Composer").HasKey(e => e.Id);

        // Properties
        builder.Property(e => e.Id).IsRequired();
        builder.Property(e => e.Name).HasMaxLength(50).IsRequired();

        // Indexes
        builder.HasIndex(e => e.Name).IsUnique().HasDatabaseName($"AK_Composer_Name");
    }
}