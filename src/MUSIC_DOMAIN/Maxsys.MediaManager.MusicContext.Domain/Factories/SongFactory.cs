using Maxsys.MediaManager.MusicContext.Domain.Enums;

namespace Maxsys.MediaManager.MusicContext.Domain.Factories;

public static class SongFactory
{
    public static Song Create(
        Guid id,
        Guid albumId,
        string originalFileName,
        string fullPath,
        string title,
        int? trackNumber,
        string? lyrics,
        string? comments,
        string? spotifyID,
        string? isrc,
        bool isBonusTrack,
        VocalGenders vocalGender,
        string? coveredArtist,
        string? featuredArtist,
        int rating,
        long fileSize,
        TimeSpan duration,
        int bitrate,
        IEnumerable<Composer>? composers = null)
    {
        // nullable strings
        lyrics = string.IsNullOrWhiteSpace(lyrics) ? null : lyrics;
        comments = string.IsNullOrWhiteSpace(comments) ? null : comments;
        coveredArtist = string.IsNullOrWhiteSpace(coveredArtist) ? null : coveredArtist;
        featuredArtist = string.IsNullOrWhiteSpace(featuredArtist) ? null : featuredArtist;

        var songDetails = new SongDetails(isBonusTrack, vocalGender, coveredArtist, featuredArtist);
        var classification = new Classification(rating);
        var songProperties = new SongProperties(duration, bitrate);

        var music = new Song(
            id,
            albumId,
            originalFileName,
            fullPath,
            fileSize,
            title,
            trackNumber,
            lyrics,
            comments,
            spotifyID,
            isrc,
            songDetails,
            classification,
            songProperties);

        // Add composers
        if (composers != null)
        {
            foreach (var composer in composers)
            {
                music.AddComposer(composer);
            }
        }

        return music;
    }

    public static Song Create(
        Guid albumId,
        string originalFileName,
        string fullPath,
        string title,
        int? trackNumber,
        string? lyrics,
        string? comments,
        string? spotifyID,
        string? isrc,
        bool isBonusTrack,
        VocalGenders vocalGender,
        string? coveredArtist,
        string? featuredArtist,
        byte stars10,
        long fileSize,
        TimeSpan duration,
        int bitrate,
        IEnumerable<Composer>? composers = null)
        => Create(GuidGen.NewSequentialGuid(), albumId, originalFileName, fullPath, title, trackNumber, lyrics, comments,
            spotifyID, isrc, isBonusTrack, vocalGender, coveredArtist, featuredArtist, stars10,
            fileSize, duration, bitrate, composers);
}