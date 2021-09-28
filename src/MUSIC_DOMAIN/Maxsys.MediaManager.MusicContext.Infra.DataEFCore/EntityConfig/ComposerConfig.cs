using Maxsys.MediaManager.MusicContext.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Maxsys.MediaManager.MusicContext.Infra.DataEFCore.EntityConfig
{
    internal class ComposerConfig : IEntityTypeConfiguration<Composer>
    {
		public void Configure(EntityTypeBuilder<Composer> builder)
		{
			builder.ToTable("Composers");

			builder.HasKey(composer => composer.Id);

			// Properties
			builder.Property(composer => composer.Name)
				.HasMaxLength(50)
				.IsRequired();

			// already config in MusicConfig
			// builder.HasMany(composer => composer.Musics).WithMany(music => music.Composers);

			// Indexes
			builder.HasIndex(composer => composer.Name)
				.IsUnique().HasDatabaseName($"AK_Composers_Name");
		}
    }
}
