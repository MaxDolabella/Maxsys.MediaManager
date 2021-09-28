using Maxsys.MediaManager.MusicContext.Domain.Entities;
using Maxsys.MediaManager.MusicContext.Domain.ValueObjects;
using System;

namespace Maxsys.MediaManager.MusicContext.Domain.Factories
{
    public static class AlbumFactory
    {
        /// <summary>
        /// Factory for Album
        /// </summary>
        /// <param name="albumDirectory">is the artist directory in library (based on <see cref="Artist"/> properties)</param>
        /// <param name="name">Required. Album name.</param>
        /// <param name="year">Optional. Album year</param>
        /// <param name="genre">Required. Album genre.</param>
        /// <param name="albumCover">Album cover picture.</param>
        /// <param name="albumType">Album type.</param>
        /// <param name="artistId">Required. Id of Album artist.</param>
        public static Album Create(Guid id, string albumDirectory, string name, int? year, string genre, byte[] albumCover, AlbumType albumType, Guid artistId)
        {
            return new Album(id, albumDirectory, name, year, genre, albumCover, albumType, artistId);
        }
    }
}