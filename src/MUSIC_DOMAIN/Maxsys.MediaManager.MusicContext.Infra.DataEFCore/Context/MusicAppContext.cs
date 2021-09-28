using Maxsys.MediaManager.CoreDomain;
using Maxsys.MediaManager.MusicContext.Domain.Entities;
using Maxsys.MediaManager.MusicContext.Domain.ValueObjects;
using Maxsys.MediaManager.MusicContext.Infra.DataEFCore.EntityConfig;
using Microsoft.EntityFrameworkCore;
using ModelBuilderExtensions;
using System;
using System.Data;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Maxsys.MediaManager.MusicContext.Infra.DataEFCore.Context
{
    public class MusicAppContext : DbContext
    {
        #region CTOR

        public MusicAppContext(DbContextOptions<MusicAppContext> options)
                    : base(options)
        {
            ChangeTracker.LazyLoadingEnabled = false;
            //ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTrackingWithIdentityResolution;
        }

        #endregion CTOR

        #region OVERRIDES

        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    optionsBuilder.UseSqlServer(_connection);
        //}

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            #region Custom Conventions

            modelBuilder.DateTimeToDateConvention();
            //modelBuilder.StringToVarcharConvention();
            //modelBuilder.StringMaxLength100Convention();

            #endregion Custom Conventions

            #region Configurations

            modelBuilder.Ignore<MusicDetails>();
            modelBuilder.Ignore<Classification>();
            modelBuilder.Ignore<MusicProperties>();
            modelBuilder.Ignore<MusicRank>();
            // modelBuilder.Ignore<PlaylistItemCollection>();

            modelBuilder.ApplyConfiguration(new MusicCatalogConfig());
            modelBuilder.ApplyConfiguration(new ArtistConfig());
            modelBuilder.ApplyConfiguration(new AlbumConfig());
            modelBuilder.ApplyConfiguration(new ComposerConfig());
            modelBuilder.ApplyConfiguration(new PlaylistItemConfig());
            modelBuilder.ApplyConfiguration(new PlaylistConfig());
            modelBuilder.ApplyConfiguration(new MusicConfig());

            #endregion Configurations
        }

        public override int SaveChanges()
        {
            SetCreatedAndUpdateDates();
            //ConfigureDateTimeValueToSqlDateTimeMinAndMaxValues();

            return base.SaveChanges();
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            SetCreatedAndUpdateDates();
            //ConfigureDateTimeValueToSqlDateTimeMinAndMaxValues();

            return (await base.SaveChangesAsync(true, cancellationToken));
        }

        #endregion OVERRIDES

        #region DbSets

        public DbSet<MusicCatalog> MusicCatalogs { get; set; }
        public DbSet<Artist> Artists { get; set; }
        public DbSet<Album> Albums { get; set; }
        public DbSet<Composer> Composers { get; set; }
        public DbSet<Music> Musics { get; set; }
        public DbSet<Playlist> Playlists { get; set; }

        #endregion DbSets

        #region PRIVATE METHODS

        private void SetCreatedAndUpdateDates()
        {
            //*
            var entries = ChangeTracker.Entries()
                .Where(e => e.Entity is MediaFile && (
                        e.State == EntityState.Added
                        || e.State == EntityState.Modified));

            #region MediaFile.CreatedDate e MediaFile.UpdatedDate

            var dateNow = DateTime.Now;
            foreach (var entityEntry in entries)
            {
                ((MediaFile)entityEntry.Entity).SetUpdatedDate(dateNow);

                if (entityEntry.State == EntityState.Added)
                {
                    ((MediaFile)entityEntry.Entity).SetCreatedDate(dateNow);
                }
            }

            #endregion MediaFile.CreatedDate e MediaFile.UpdatedDate

            //*/
        }

        /// <summary>
        /// Configure all <see cref="DateTime"/> objects to be between <see cref="System.Data.SqlTypes.SqlDateTime.MinValue"/> and <see cref="System.Data.SqlTypes.SqlDateTime.MaxValue"/>
        /// </summary>
        private void ConfigureDateTimeValueToSqlDateTimeMinAndMaxValues()
        {
            //*
            foreach (var change in ChangeTracker.Entries<object>())
            {
                var props = change.Properties;
                foreach (var prop in props)
                {
                    //var propName = prop.Metadata.Name;
                    var value = prop.CurrentValue;
                    if (value is DateTime)
                    {
                        var date = (DateTime)value;
                        if (date < System.Data.SqlTypes.SqlDateTime.MinValue.Value)
                        {
                            prop.CurrentValue = System.Data.SqlTypes.SqlDateTime.MinValue.Value;
                        }
                        else if (date > System.Data.SqlTypes.SqlDateTime.MaxValue.Value)
                        {
                            prop.CurrentValue = System.Data.SqlTypes.SqlDateTime.MaxValue.Value;
                        }
                    }
                }
            }
            //*/
        }

        #endregion PRIVATE METHODS
    }
}