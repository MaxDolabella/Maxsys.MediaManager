using Maxsys.MediaManager.MusicContext.Domain.Enums;

namespace Maxsys.MediaManager.DatabaseInitializer;

internal record CatalogJson(Guid Id, string Name);
internal record ArtistJson(Guid Id, Guid MusicCatalogId, string Name);
internal record ArtistSpotifyJson(Guid Id, string SpotifyId);

internal record AlbumJson(Guid Id, Guid ArtistId, string AlbumDirectory, string Name, short? Year, string Genre, AlbumTypes AlbumType);
internal record AlbumCoverJson(Guid Id, byte[] AlbumCover);

internal record ComposerJson(Guid Id, string Name);
internal record ComposerSongJson(Guid ComposersId, Guid MusicsId);

internal record SongJson(Guid Id, Guid AlbumId, string Title, int? TrackNumber, int Classification_Rating,
    string? Lyrics, string? Comments,
    bool MusicDetails_IsBonusTrack, VocalGenders MusicDetails_VocalGender, 
    string? MusicDetails_CoveredArtist, string? MusicDetails_FeaturedArtist,
    TimeSpan MusicProperties_Duration, int MusicProperties_BitRate,
    string MediaFile_FullPath, string MediaFile_OriginalFileName, long MediaFile_FileSize,
    DateTime MediaFile_CreatedDate, DateTime? MediaFile_UpdatedDate);

internal record PlaylistJson(Guid Id, string Name);
internal record PlaylistItemJson(Guid PlaylistId, Guid MusicId, short Order);