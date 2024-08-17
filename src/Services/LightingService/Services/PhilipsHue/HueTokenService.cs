using HomeNet.Core.Dtos.KeyVault;
using HomeNet.Services.LightingService.Services.Models;
using HomeNet.Services.Shared.Services.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using HomeNet.Services.LightingService.Services.Interfaces;

namespace HomeNet.Services.LightingService.Services.PhilipsHue
{
    public class HueTokenService : IHueTokenService
    {
        private readonly ILogger<HueTokenService> _logger;
        private readonly HttpClient _httpClient;
        private readonly IKeyVaultServiceClient _keyVaultServiceClient;

        public HueTokenService(ILogger<HueTokenService> logger, HttpClient httpClient, IKeyVaultServiceClient keyVaultServiceClient)
        {
            _logger = logger;
            _httpClient = httpClient;
            _keyVaultServiceClient = keyVaultServiceClient;
        }

        public async Task<string> RefreshAccessTokenAsync()
        {
            var credentials = await FetchRefreshCredentialsAsync();

            var authHeader = Convert.ToBase64String(Encoding.ASCII.GetBytes($"{credentials.ClientId}:{credentials.ClientSecret}"));

            var request = new HttpRequestMessage(HttpMethod.Post, "/oauth2/refresh?grant_type=refresh_token")
            {
                Headers = { Authorization = new AuthenticationHeaderValue("Basic", authHeader) },
                Content = new FormUrlEncodedContent(new Dictionary<string, string>
            {
            { "refresh_token", credentials.RefreshToken }
            })
            };

            var response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();

            var responseContent = await response.Content.ReadAsStringAsync();

            var tokenResponse = JsonSerializer.Deserialize<AccessTokenResponse>(responseContent);

            if (tokenResponse == null || !tokenResponse.IsValid())
            {
                throw new Exception("Invalid token response");
            }

            await StoreTokensInKeyVaultAsync(tokenResponse);

            return tokenResponse.AccessToken;
        }


        private async Task StoreTokensInKeyVaultAsync(AccessTokenResponse tokenResponse)
        {
            var accessTokenExpiresOn = DateTimeOffset.UtcNow.AddSeconds(int.Parse(tokenResponse.AccessTokenExpiresIn));
            var refreshTokenExpiresOn = DateTimeOffset.UtcNow.AddSeconds(int.Parse(tokenResponse.RefreshTokenExpiresIn));

            await _keyVaultServiceClient.SetSecretAsync(new CreateSecretDto
            {
                ResourceType = "PhilipsHue",
                KeyType = "AccessToken",
                Value = tokenResponse.AccessToken,
                ExpiresOn = accessTokenExpiresOn
            });

            await _keyVaultServiceClient.SetSecretAsync(new CreateSecretDto
            {
                ResourceType = "PhilipsHue",
                KeyType = "RefreshToken",
                Value = tokenResponse.RefreshToken,
                ExpiresOn = refreshTokenExpiresOn
            });
        }

        private async Task<RefreshAccessTokenCredentials> FetchRefreshCredentialsAsync()
        {
            var clientId = await _keyVaultServiceClient.GetSecretValueAsync(new GetSecretDto
            {
                ResourceType = "PhilipsHue",
                KeyType = "ClientId"
            });

            var clientSecret = await _keyVaultServiceClient.GetSecretValueAsync(new GetSecretDto
            {
                ResourceType = "PhilipsHue",
                KeyType = "ClientSecret"
            });

            var refreshToken = await _keyVaultServiceClient.GetSecretValueAsync(new GetSecretDto
            {
                ResourceType = "PhilipsHue",
                KeyType = "RefreshToken"
            });

            if (string.IsNullOrEmpty(clientId))
            {
                throw new ArgumentNullException(nameof(clientId), "Client ID cannot be null or empty.");
            }

            if (string.IsNullOrEmpty(clientSecret))
            {
                throw new ArgumentNullException(nameof(clientSecret), "Client Secret cannot be null or empty.");
            }

            if (string.IsNullOrEmpty(refreshToken))
            {
                throw new ArgumentNullException(nameof(refreshToken), "Refresh Token cannot be null or empty.");
            }

            return new RefreshAccessTokenCredentials
            {
                ClientId = clientId,
                ClientSecret = clientSecret,
                RefreshToken = refreshToken
            };
        }
    }
}
