using Maxsys.MediaManager.MusicContext.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Maxsys.MediaManager.MusicContext.Infra.DataEFCore.EntityConfig
{
    internal class MusicCatalogConfig : IEntityTypeConfiguration<MusicCatalog>
    {
		public void Configure(EntityTypeBuilder<MusicCatalog> builder)
		{
			builder.ToTable("MusicCatalogs");

			builder.HasKey(musicCatalog => musicCatalog.Id);

			builder.Property(musicCatalog => musicCatalog.Name)
				.HasMaxLength(50)
				.IsRequired();

			// Indexes
			builder.HasIndex(musicCatalog => musicCatalog.Name)
				.IsUnique().HasDatabaseName("AK_MusicCatalogs_Name");
		}
    }
}
