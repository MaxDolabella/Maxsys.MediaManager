using System.Reflection;
using System.Text.Json;
using Maxsys.MediaManager.DatabaseInitializer.Extensions;
using Maxsys.MediaManager.MusicContext.Domain.Entities;
using Maxsys.MediaManager.MusicContext.Domain.Enums;
using Maxsys.MediaManager.MusicContext.Domain.Factories;
using Microsoft.EntityFrameworkCore;

namespace Maxsys.MediaManager.DatabaseInitializer;

internal static class Seed
{
    private static Assembly Assembly { get; } = Assembly.GetExecutingAssembly();

    public static async Task AsyncSeeding(DbContext context, bool _, CancellationToken cancellationToken)
    {
        if (!await context.Set<Catalog>().AnyAsync(cancellationToken))
        {
            #region Catalog

            var catalogs = GetJsonItems<CatalogJson, Catalog>("Catalog",
                x => CatalogFactory.Create(x.Id, x.Name));

            await context.Set<Catalog>().AddRangeAsync(catalogs, cancellationToken);

            #endregion Catalog

            #region Artist

            var artistsJson = GetJsonItems<ArtistJson>("Artist");
            var artistsSpotifyJson = GetJsonItems<ArtistSpotifyJson>("ArtistSpotifyIds");

            List<Artist> artists = [];
            foreach (var item in artistsJson)
            {
                var artist = ArtistFactory.Create(item.Id, item.MusicCatalogId, item.Name, null);
                artist.SetSpotifyID(artistsSpotifyJson.FirstOrDefault(x => x.Id == item.Id)?.SpotifyId);

                artists.Add(artist);
            }

            await context.Set<Artist>().AddRangeAsync(artists, cancellationToken);

            #endregion Artist

            #region Album

            var albumsCoverJson = GetJsonItems<AlbumCoverJson>("AlbumCover");
            var albums = GetJsonItems<AlbumJson, Album>("Album", json =>
                AlbumFactory.Create(json.Id, json.ArtistId, json.AlbumDirectory, json.Name, shortName: null,
                    json.Year, json.Genre, albumsCoverJson.First(x => x.Id == json.Id).AlbumCover,
                    (AlbumTypes)json.AlbumType, null));

            await context.Set<Album>().AddRangeAsync(albums, cancellationToken);

            #endregion Album

            #region Composer

            var composers = GetJsonItems<ComposerJson, Composer>("Composer", json =>
                ComposerFactory.Create(json.Id, json.Name));

            await context.Set<Composer>().AddRangeAsync(composers, cancellationToken);

            #endregion Composer

            #region Song

            var songs = GetJsonItems<SongJson, Song>("Song", json =>
            {
                var song = SongFactory.Create(json.Id, json.AlbumId, json.MediaFile_OriginalFileName, json.MediaFile_FullPath
                    , json.Title, json.TrackNumber, json.Lyrics, json.Comments, spotifyID: null, isrc: null, json.MusicDetails_IsBonusTrack
                    , json.MusicDetails_VocalGender, json.MusicDetails_CoveredArtist, json.MusicDetails_FeaturedArtist, json.Classification_Rating
                    , json.MediaFile_FileSize, json.MusicProperties_Duration, json.MusicProperties_BitRate, null);

                song.SetCreatedAt(json.MediaFile_CreatedDate);
                if (json.MediaFile_UpdatedDate is not null && json.MediaFile_UpdatedDate.Value != json.MediaFile_CreatedDate)
                {
                    song.SetLastUpdateAt(json.MediaFile_UpdatedDate.Value);
                }

                return song;
            });

            await context.Set<Song>().AddRangeAsync(songs, cancellationToken);

            #endregion Song

            #region Playlist

            var playlists = GetJsonItems<PlaylistJson, Playlist>("Playlist", json =>
                PlaylistFactory.Create(json.Id, json.Name, null));

            await context.Set<Playlist>().AddRangeAsync(playlists, cancellationToken);

            #endregion Playlist

            #region PlaylistItem

            var playlistItems = GetJsonItems<PlaylistItemJson, PlaylistItem>("PlaylistItem", json =>
                new PlaylistItem(json.PlaylistId, json.MusicId, json.Order == 0 ? null : json.Order));

            await context.Set<PlaylistItem>().AddRangeAsync(playlistItems, cancellationToken);

            #endregion PlaylistItem

            var composerSongsInsertValues = GetJsonItems<ComposerSongJson>("ComposerSong").Select(x => $"('{x.ComposersId}', '{x.MusicsId}')").Chunk(1000);
            var composerSongsInsertScripts = composerSongsInsertValues.Select(x => $"INSERT INTO ComposerSong VALUES {string.Join(',', x)}");

            await context.SaveChangesAsync(cancellationToken);
            foreach (var script in composerSongsInsertScripts)
            {
                await context.Database.ExecuteSqlRawAsync(script);
            }
        }
    }

    private static IEnumerable<TJsonModel> GetJsonItems<TJsonModel>(string jsonFile)
    {
        return JsonSerializer.Deserialize<TJsonModel[]>(Assembly.GetSeedJson(jsonFile)!)!;
    }

    private static IEnumerable<TEntity> GetJsonItems<TJsonModel, TEntity>(string jsonFile, Func<TJsonModel, TEntity> convertion)
    {
        var jsonItems = GetJsonItems<TJsonModel>(jsonFile);

        return jsonItems.Select(convertion);
    }
}