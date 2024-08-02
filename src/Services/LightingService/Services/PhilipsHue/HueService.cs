using HomeNet.Core.Dtos.KeyVault;
using HomeNet.Core.Dtos.Lighting;
using HomeNet.Services.LightingService.Services.Interfaces;
using HomeNet.Services.Shared.Services.Interfaces;
using Microsoft.Extensions.Logging;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

//https://ashiqf.com/tag/philips-hue-remote-api/

namespace HomeNet.Services.LightingService.Services.PhilipsHue
{
    public class HueService : IHueService
    {
        private readonly ILogger<HueService> _logger;
        private readonly HttpClient _httpClient;
        private readonly IKeyVaultServiceClient _keyVaultServiceClient;

        public HueService(ILogger<HueService> logger ,HttpClient httpClient, IKeyVaultServiceClient keyVaultServiceClient)
        {
            _logger = logger;
            _httpClient = httpClient;
            _keyVaultServiceClient = keyVaultServiceClient;
        }

        public async Task<string> GetLightsAsync()
        {
            var username = await _keyVaultServiceClient.GetSecretValueAsync(new GetSecretDto
            {
                ResourceType = "PhilipsHue",
                KeyType = "Username"
            });

            var accessToken = await _keyVaultServiceClient.GetSecretValueAsync(new GetSecretDto
            {
                ResourceType = "PhilipsHue",
                KeyType = "AccessToken"
            });

            var request = new HttpRequestMessage(HttpMethod.Get, $"/bridge/{username}/lights/");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            var response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadAsStringAsync();
        }

        public async Task<string> SetLightStateAsync(string lightNumber, bool isOn)
        {
            var username = await _keyVaultServiceClient.GetSecretValueAsync(new GetSecretDto
            {
                ResourceType = "PhilipsHue",
                KeyType = "Username"
            });

            var accessToken = await _keyVaultServiceClient.GetSecretValueAsync(new GetSecretDto
            {
                ResourceType = "PhilipsHue",
                KeyType = "AccessToken"
            });

            if (accessToken == null)
            {
                _logger.LogInformation("Access token is null or expired, refreshing token");
                accessToken = await RefreshAccessTokenAsync();
            }
            
            var request = new HttpRequestMessage(HttpMethod.Put, $"/bridge/{username}/lights/{lightNumber}/state");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var body = new LightStateDto { on = isOn };
            var content = new StringContent(JsonSerializer.Serialize(body), Encoding.UTF8, "application/json");

            request.Content = content;

            var response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadAsStringAsync();
        }

        private async Task<string> RefreshAccessTokenAsync()
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

            var request = new HttpRequestMessage(HttpMethod.Post, "/oauth2/refresh?grant_type=refresh_token");
            var authHeader = Convert.ToBase64String(Encoding.ASCII.GetBytes($"{clientId}:{clientSecret}"));
            request.Headers.Authorization = new AuthenticationHeaderValue("Basic", authHeader);
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/x-www-form-urlencoded"));

            var body = new StringContent($"refresh_token={refreshToken}", Encoding.UTF8, "application/x-www-form-urlencoded");
            request.Content = body;

            var response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();

            var responseContent = await response.Content.ReadAsStringAsync();

            var jsonDoc = JsonDocument.Parse(responseContent);
            var newAccessToken = jsonDoc.RootElement.GetProperty("access_token").GetString();
            var expiresIn = jsonDoc.RootElement.GetProperty("access_token_expires_in").GetString();
            
            if (newAccessToken == null || expiresIn == null)
            {
                throw new Exception("Failed to retrieve access token");
            }

            var expiresOn = DateTimeOffset.UtcNow.AddSeconds(int.Parse(expiresIn));

            await _keyVaultServiceClient.SetSecretAsync(new CreateSecretDto
            {
                ResourceType = "PhilipsHue",
                KeyType = "AccessToken",
                Value = newAccessToken,
                ExpiresOn = expiresOn
            });

            return newAccessToken;
        }
    }
}
