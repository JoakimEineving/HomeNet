using HomeNet.Core.Dtos.KeyVault;
using HomeNet.Services.Shared.Services.Interfaces;
using HomeNet.Services.WeatherService.Services.Interfaces;

namespace HomeNet.Services.WeatherService.Services.Services
{
    public class WeatherService : IWeatherService
    {
        private readonly HttpClient _httpClient;
        private readonly IKeyVaultServiceClient _keyVaultServiceClient;

        public WeatherService(HttpClient httpClient, IKeyVaultServiceClient keyVaultServiceClient)
        {
            _httpClient = httpClient;
            _keyVaultServiceClient = keyVaultServiceClient;
        }

        //TODO: Attach secrets to configuration at startup
        public async Task<string> GetWeatherAsync(double latitude, double longitude)
        {
            var apiKey = await _keyVaultServiceClient.GetSecretValueAsync(new GetSecretDto
            {
                ResourceType = "WeatherService",
                KeyType = "ApiKey"
            });

            var baseUrl = await _keyVaultServiceClient.GetSecretValueAsync(new GetSecretDto
            {
                ResourceType = "WeatherService",
                KeyType = "BaseUrl"
            });


            var request = $"?lat={latitude}&lon={longitude}&units=metric&appid={apiKey}";

            var response = await _httpClient.GetAsync(request);
            response.EnsureSuccessStatusCode();

            //TODO: Add mapper to map response to a DTO

            return await response.Content.ReadAsStringAsync();
        }

    }
}
