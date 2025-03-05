using Maxsys.Core;
using Maxsys.Core.Interfaces.Services;
using Maxsys.MediaManager.Spotify.Searching.Filters;
using Maxsys.MediaManager.Spotify.Searching.Responses;

namespace Maxsys.MediaManager.Spotify.Searching;

public interface ISearchService : IService
{
    Task<OperationResult<AlbumSearchResponse?>> SearchAlbumAsync(AlbumFilters filters, CancellationToken cancellationToken);
    Task<OperationResult<ArtistSearchResponse?>> SearchArtistAsync(ArtistFilters filters, CancellationToken cancellationToken);
    Task<OperationResult<TrackSearchResponse?>> SearchTrackAsync(TrackFilters filters, CancellationToken cancellationToken);
}