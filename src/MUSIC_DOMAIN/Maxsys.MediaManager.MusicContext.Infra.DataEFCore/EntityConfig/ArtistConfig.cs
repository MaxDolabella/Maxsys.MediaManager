using Maxsys.MediaManager.MusicContext.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Maxsys.MediaManager.MusicContext.Infra.DataEFCore.EntityConfig
{
    internal class ArtistConfig : IEntityTypeConfiguration<Artist>
    {
		public void Configure(EntityTypeBuilder<Artist> builder)
		{
			builder.ToTable("Artists");

			builder.HasKey(artist => artist.Id);


			// Properties
			builder.Property(artist => artist.Name)
				.HasMaxLength(50)
				.IsRequired();


			// Navigation
			builder.HasOne(artist => artist.MusicCatalog)
				.WithMany(catalog => catalog.Artists);

			// Indexes
			builder.HasIndex(artist => new { artist.Name, artist.MusicCatalogId })
				.IsUnique().HasDatabaseName($"AK_Artists_Name_Catalog");
		}
    }
}
