using System.Data;
using ModelBuilderExtensions;

namespace Maxsys.MediaManager.MusicContext.Infra.DataEFCore.Context;

public class MusicAppContext : DbContext
{
    #region CTOR

    public MusicAppContext(DbContextOptions<MusicAppContext> options) : base(options)
    {
        ChangeTracker.LazyLoadingEnabled = false;
        ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTrackingWithIdentityResolution;
    }

    #endregion CTOR

    #region OVERRIDES

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        #region Custom Conventions

        modelBuilder.DateTimeToDateConvention();
        modelBuilder.StringToVarcharConvention();
        //modelBuilder.StringMaxLength100Convention();

        #endregion Custom Conventions

        #region Configurations

        modelBuilder.Ignore<SongDetails>();
        modelBuilder.Ignore<Classification>();
        modelBuilder.Ignore<SongProperties>();
        // modelBuilder.Ignore<PlaylistItemCollection>();

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(MusicAppContext).Assembly);

        #endregion Configurations
    }

    public override int SaveChanges()
    {
        SetCreatedAndUpdateDates();

        return base.SaveChanges();
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        SetCreatedAndUpdateDates();

        return (await base.SaveChangesAsync(true, cancellationToken));
    }

    #endregion OVERRIDES

    #region DbSets

    public DbSet<Catalog> Catalogs { get; set; }
    public DbSet<Artist> Artists { get; set; }
    public DbSet<Album> Albums { get; set; }
    public DbSet<Composer> Composers { get; set; }
    public DbSet<Song> Songs { get; set; }
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

    #endregion PRIVATE METHODS
}