using Maxsys.MediaManager.MusicContext.Domain.Entities;
using System;

namespace Maxsys.MediaManager.MusicContext.Domain.Factories
{
    public static class MusicRankFactory
    {
        /// <summary>Factory to create a new Music.</summary>
        /// <param name="id"></param>
        /// <param name="fullPath"></param>
        /// <param name="title"></param>
        /// <param name="artistName"></param>
        /// <param name="albumName"></param>
        /// <param name="ratingPoints"></param>
        /// <returns></returns>
        public static MusicRank Create(Guid id, string fullPath, string title, string artistName, string albumName, int ratingPoints)
        {
            return new MusicRank(id, fullPath, title, artistName, albumName, ratingPoints);
        }
    }
}