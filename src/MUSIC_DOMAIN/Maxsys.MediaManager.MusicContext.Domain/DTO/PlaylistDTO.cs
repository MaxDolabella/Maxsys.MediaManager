using System;

namespace Maxsys.MediaManager.MusicContext.Domain.DTO
{
    public struct PlaylistDTO
    {
        public Guid PlaylistId { get; init; }
        public string PlaylistName { get; init; }

        public bool IsValid()
        {
            return !(PlaylistId == default || string.IsNullOrWhiteSpace(PlaylistName));
        }
    }
}
