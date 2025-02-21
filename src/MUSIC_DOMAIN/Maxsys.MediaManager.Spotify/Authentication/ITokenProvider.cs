using Maxsys.Core;
using Maxsys.Core.Interfaces.Services;

namespace Maxsys.MediaManager.Spotify.Authentication;

public interface ITokenProvider : IService
{
    Task<OperationResult<AccessToken?>> GetAccessTokenAsync(CancellationToken cancellationToken);
}