using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Maxsys.MediaManager.MusicContext.Infra.DataEFCore.EntityConfig;

internal class ComposerConfig : IEntityTypeConfiguration<Composer>
{
    public void Configure(EntityTypeBuilder<Composer> builder)
    {
        builder.ToTable("Composers")
            .HasKey(composer => composer.Id);

        // Properties
        builder.Property(composer => composer.Name)
            .HasMaxLength(50)
            .IsRequired();

        // Indexes
        builder.HasIndex(composer => composer.Name)
            .IsUnique().HasDatabaseName($"AK_Composers_Name");
    }
}