using HomeNet.Core.Dtos.KeyVault;
using HomeNet.Core.Dtos.Lighting;
using HomeNet.Services.LightingService.Services.Interfaces;
using HomeNet.Services.LightingService.Services.Models;
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
        private readonly IHueTokenService _hueTokenService;

        public HueService(ILogger<HueService> logger, HttpClient httpClient, IKeyVaultServiceClient keyVaultServiceClient, IHueTokenService hueTokenService)
        {
            _logger = logger;
            _httpClient = httpClient;
            _keyVaultServiceClient = keyVaultServiceClient;
            _hueTokenService=hueTokenService;
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
                accessToken = await _hueTokenService.RefreshAccessTokenAsync();
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
    }
}
