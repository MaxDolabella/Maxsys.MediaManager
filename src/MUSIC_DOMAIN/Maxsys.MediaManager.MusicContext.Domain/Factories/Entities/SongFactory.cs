﻿namespace Maxsys.MediaManager.MusicContext.Domain.Factories;

public static class SongFactory
{
    public static Song Create(
        Guid albumId,
        string originalFileName,
        string fullPath,
        string title,
        int? trackNumber,
        string? lyrics,
        string? comments,
        bool isBonusTrack,
        VocalGender vocalGender,
        string? coveredArtist,
        string? featuredArtist,
        byte stars10,
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
        var classification = new Classification(stars10);
        var songProperties = new SongProperties(duration, bitrate);

        var music = new Song(
            GuidGen.NewSequentialGuid(),
            albumId,
            originalFileName,
            fullPath,
            fileSize,
            title,
            trackNumber,
            lyrics,
            comments,
            songDetails,
            classification,
            songProperties);

        // Add composers
        if (composers != null)
            foreach (var composer in composers)
                music.AddComposer(composer);

        return music;
    }
}