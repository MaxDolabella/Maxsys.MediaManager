using System.Net.Http.Headers;
using System.Text;
using Maxsys.Core;
using Maxsys.Core.Extensions;
using Maxsys.Core.Services.Http;
using Maxsys.MediaManager.CoreDomain.Common;
using Maxsys.MediaManager.Spotify.Authentication;
using Maxsys.MediaManager.Spotify.Options;
using Maxsys.MediaManager.Spotify.Searching.Filters;
using Maxsys.MediaManager.Spotify.Searching.Responses;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Maxsys.MediaManager.Spotify.Searching;

public class SearchService : HttpClientBase, ISearchService
{
    private const string ROUTE = "v1/search";
    private const string MARKET = "BR";
    private readonly ITokenProvider _tokenProvider;

    public SearchService(ILogger<SearchService> logger, IHttpClientFactory httpClientFactory, IOptions<SpotifySettings> options, ITokenProvider tokenProvider)
        : base(logger, httpClientFactory)
    {
        _tokenProvider = tokenProvider;
        _httpClient.BaseAddress = options.Value.ApiUri;
    }

    public async Task<OperationResult<ArtistSearchResponse?>> SearchArtistAsync(ArtistFilters filters, CancellationToken cancellationToken)
    {
        var queryParams = new QueryParamsBuilder()
            .Add("q", filters.Term, false)
            .Add("type", SearchOptions.Artist.ToString().ToLower())
            .Add("market", MARKET)
            .Add("limit", filters.Pagination.Size)
            .Add("offset", filters.Pagination.Index)
            .ToString();

        var route = $"{ROUTE}{queryParams}";

        var requestMessage = await GetHttpRequestMessageAsync(HttpMethod.Get, route, null, cancellationToken);
        var responseMessage = await _httpClient.SendAsync(requestMessage, cancellationToken);
        var responseContent = await responseMessage.Content.ReadAsStringAsync(cancellationToken);

        if (!responseMessage.IsSuccessStatusCode)
        {
            var error = responseContent.FromJson<ErrorResponse>();
            return new(new Notification(Messages.SPOTIFY_ERROR, error.Message, ResultTypes.Error));
        }

        if (!responseContent.TryFromJson<SearchResponse>(out var result, out var deserializeError))
        {
            return new(deserializeError!);
        }

        return new(result!.Artists);
    }

    public async Task<OperationResult<AlbumSearchResponse?>> SearchAlbumAsync(AlbumFilters filters, CancellationToken cancellationToken)
    {
        var queryTerm = new StringBuilder(filters.Term);

        if (!string.IsNullOrWhiteSpace(filters.Artist))
        {
            //remaster track:Doxy artist:Miles Davis
            queryTerm.AppendFormat("%20artist:{0}", filters.Artist.Replace(' ', '+'));
        }

        if (filters.Year.HasValue)
        {
            //remaster track:Doxy artist:Miles Davis
            queryTerm.AppendFormat("%20year:{0}", filters.Year);
        }

        var queryParams = new QueryParamsBuilder()
            .Add("q", queryTerm.ToString(), false)
            .Add("type", SearchOptions.Album.ToString().ToLower())
            .Add("market", MARKET)
            .Add("limit", filters.Pagination.Size)
            .Add("offset", filters.Pagination.Index)
            .ToString();

        var route = $"{ROUTE}{queryParams}";

        var requestMessage = await GetHttpRequestMessageAsync(HttpMethod.Get, route, null, cancellationToken);
        var responseMessage = await _httpClient.SendAsync(requestMessage, cancellationToken);
        var responseContent = await responseMessage.Content.ReadAsStringAsync(cancellationToken);

        if (!responseMessage.IsSuccessStatusCode)
        {
            var error = responseContent.FromJson<ErrorResponse>();
            return new(new Notification(Messages.SPOTIFY_ERROR, error.Message, ResultTypes.Error));
        }

        if (!responseContent.TryFromJson<SearchResponse>(out var result, out var deserializeError))
        {
            return new(deserializeError!);
        }

        return new(result!.Albums);
    }

    public async Task<OperationResult<TrackSearchResponse?>> SearchTrackAsync(TrackFilters filters, CancellationToken cancellationToken)
    {
        var queryTerm = new StringBuilder(filters.Term);

        if (!string.IsNullOrWhiteSpace(filters.Artist))
        {
            //remaster track:Doxy artist:Miles Davis
            queryTerm.AppendFormat("%20artist:{0}", filters.Artist);
        }

        if (!string.IsNullOrWhiteSpace(filters.Album))
        {
            //remaster track:Doxy artist:Miles Davis
            queryTerm.AppendFormat("%20album:{0}", filters.Album);
        }

        if (!string.IsNullOrWhiteSpace(filters.ISRC))
        {
            //remaster track:Doxy artist:Miles Davis
            queryTerm.AppendFormat("%20isrc:{0}", filters.ISRC);
        }

        if (filters.Year.HasValue)
        {
            //remaster track:Doxy artist:Miles Davis
            queryTerm.AppendFormat("%20year:{0}", filters.Year);
        }

        var queryParams = new QueryParamsBuilder()
            .Add("q", queryTerm.ToString(), false)
            .Add("type", SearchOptions.Track.ToString().ToLower())
            .Add("market", MARKET)
            .Add("limit", filters.Pagination.Size)
            .Add("offset", filters.Pagination.Index)
            .ToString();

        var route = $"{ROUTE}{queryParams}";

        var requestMessage = await GetHttpRequestMessageAsync(HttpMethod.Get, route, null, cancellationToken);
        var responseMessage = await _httpClient.SendAsync(requestMessage, cancellationToken);
        var responseContent = await responseMessage.Content.ReadAsStringAsync(cancellationToken);

        if (!responseMessage.IsSuccessStatusCode)
        {
            var error = responseContent.FromJson<ErrorResponse>();
            return new(new Notification(Messages.SPOTIFY_ERROR, error.Message, ResultTypes.Error));
        }

        if (!responseContent.TryFromJson<SearchResponse>(out var result, out var deserializeError))
        {
            return new(deserializeError!);
        }

        return new(result!.Tracks);
    }

    protected override async ValueTask<AuthenticationHeaderValue?> AddAuthTokenAsync(CancellationToken cancellationToken = default)
    {
        var result = await _tokenProvider.GetAccessTokenAsync(cancellationToken);
        if (!result.IsValid)
        {
            throw new Maxsys.Core.Exceptions.ExternalAPIException(System.Net.HttpStatusCode.Forbidden, result.ToString());
        }

        return new AuthenticationHeaderValue("Bearer", result.Data.Value);
    }
}