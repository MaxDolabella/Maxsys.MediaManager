using Maxsys.MediaManager.MusicContext.Domain.Entities;
using Maxsys.MediaManager.MusicContext.Domain.ValueObjects;
using System;
using System.Collections.Generic;

namespace Maxsys.MediaManager.MusicContext.Domain.Factories
{
    public static class MusicFactory
    {
        public static Music Create(
            Guid id,
            Guid albumId,
            string originalFileName,
            string fullPath,
            string title,
            int? trackNumber,
            string lyrics,
            string comments,
            bool isBonusTrack,
            VocalGender vocalGender,
            string coveredArtist,
            string featuredArtist,
            byte stars10,
            long fileSize,
            TimeSpan duration,
            int bitrate,
            IEnumerable<Composer> composers = null)
        {
            // nullable strings
            lyrics = string.IsNullOrWhiteSpace(lyrics) ? null : lyrics;
            comments = string.IsNullOrWhiteSpace(comments) ? null : comments;
            coveredArtist = string.IsNullOrWhiteSpace(coveredArtist) ? null : coveredArtist;
            featuredArtist = string.IsNullOrWhiteSpace(featuredArtist) ? null : featuredArtist;

            var musicDetails = new MusicDetails(isBonusTrack, vocalGender, coveredArtist, featuredArtist);
            var classification = new Classification(stars10);
            var musicProperties = new MusicProperties(duration, bitrate);

            var music = new Music(
                id,
                albumId,
                originalFileName,
                fullPath,
                fileSize,
                title,
                trackNumber,
                lyrics,
                comments,
                musicDetails,
                classification,
                musicProperties);

            // Add composers
            if (composers != null)
                foreach (var composer in composers)
                    music.AddComposer(composer);

            return music;
        }
    }
}