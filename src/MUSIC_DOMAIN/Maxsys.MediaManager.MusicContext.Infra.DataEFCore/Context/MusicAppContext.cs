using Maxsys.Core.Data.SqlServer.Conventions;

namespace Maxsys.MediaManager.MusicContext.Infra.DataEFCore.Context;

public class MusicAppContext : DbContext
{
    public MusicAppContext(DbContextOptions<MusicAppContext> options) : base(options)
    {
        ChangeTracker.LazyLoadingEnabled = false;
        ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTrackingWithIdentityResolution;
    }

    #region OVERRIDES

    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    {
        configurationBuilder.StringToVarcharConvention(100);
        configurationBuilder.DateTimeToDateConvention();
        configurationBuilder.IgnoreDomainEventConvention();

        base.ConfigureConventions(configurationBuilder);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Ignore<SongDetails>();
        modelBuilder.Ignore<Classification>();
        modelBuilder.Ignore<SongProperties>();
        // modelBuilder.Ignore<PlaylistItemCollection>();

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(MusicAppContext).Assembly);
    }

    #endregion OVERRIDES
}