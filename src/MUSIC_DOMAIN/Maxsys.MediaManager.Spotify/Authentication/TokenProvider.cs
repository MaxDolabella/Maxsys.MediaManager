using System.Net.Http.Headers;
using System.Text;
using Maxsys.Core;
using Maxsys.Core.Extensions;
using Maxsys.Core.Services.Http;
using Maxsys.MediaManager.Spotify.Options;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Maxsys.MediaManager.Spotify.Authentication;

public class TokenProvider : HttpClientBase, ITokenProvider
{
    private readonly SpotifySettings _settings;

    public TokenProvider(ILogger<TokenProvider> logger, IHttpClientFactory httpClientFactory, IOptions<SpotifySettings> settingsOptions)
        : base(logger, httpClientFactory)
    {
        _settings = settingsOptions.Value;
    }

    public async Task<OperationResult<AccessToken?>> GetAccessTokenAsync(CancellationToken cancellationToken)
    {
        var filePath = GetAccessTokenFilePath();
        if (File.Exists(filePath))
        {
            var accessTokenFile = GetAccessTokenFromFile(filePath);
            if (accessTokenFile.IsValid())
            {
                return new(accessTokenFile);
            }

            File.Delete(filePath);
        }

        var result = await GetAuthenticationResponseAsync(cancellationToken);
        if (!result.IsValid)
        {
            return result.Cast<AccessToken?>();
        }

        var accessToken = new AccessToken
        {
            Value = result.Data.Token,
            ExpiresAt = result.Data.ExpiresAt
        };

        File.WriteAllText(filePath, accessToken.ToJson());

        return new(accessToken);
    }

    private async Task<OperationResult<AuthenticationResponse?>> GetAuthenticationResponseAsync(CancellationToken cancellationToken)
    {
        var formCollection = new Dictionary<string, string>() { ["grant_type"] = "client_credentials" };
        var content = new FormUrlEncodedContent(formCollection);
        var requestMessage = await GetHttpRequestMessageWithContentAsync(HttpMethod.Post, _settings.AuthenticationUri.AbsoluteUri, content, null, cancellationToken);
        var responseMessage = await _httpClient.SendAsync(requestMessage, cancellationToken);
        var responseContent = await responseMessage.Content.ReadAsStringAsync(cancellationToken);

        if (!responseMessage.IsSuccessStatusCode)
        {
            return new(new Notification("AUTH_ERROR", responseContent, ResultTypes.Error));
        }

        var response = responseContent.FromJson<AuthenticationResponse>();

        return new(response);
    }

    private static string GetAccessTokenFilePath()
    {
        return $"{Environment.CurrentDirectory}\\spotify.token";
    }

    private static AccessToken GetAccessTokenFromFile(string filePath)
    {
        var contents = File.ReadAllText(filePath);

        return contents.FromJson<AccessToken>();
    }

    protected override ValueTask<AuthenticationHeaderValue?> AddAuthTokenAsync(CancellationToken cancellationToken = default)
    {
        // Base 64 encode client ID and client secret key to include in Auth Header
        var stringToEncode = $"{_settings.ClientId}:{_settings.ClientSecret}";

        var utf8Bytes = Encoding.UTF8.GetBytes(stringToEncode);
        var base64 = Convert.ToBase64String(utf8Bytes);

        var authHeader = new AuthenticationHeaderValue("Basic", base64);

        return ValueTask.FromResult<AuthenticationHeaderValue?>(authHeader);

        /*

        let rawStr = CryptoJS.enc.Utf8.parse(stringToEncode)
        let base64 = CryptoJS.enc.Base64.stringify(rawStr)
        // console.log(`Encrypted value: ${base64}`)

        // set local variable to be used in Auth Header
        pm.environment.set("encodedIdAndKey", base64)

        https://accounts.spotify.com/api/token
        Form grant_type: client_credentials
        Header: Auth: Basic {{encodedIdAndKey}}

        */
    }
}