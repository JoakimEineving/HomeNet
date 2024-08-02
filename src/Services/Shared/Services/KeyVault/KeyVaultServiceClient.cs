using HomeNet.Core.Dtos.KeyVault;
using HomeNet.Services.Shared.Services.Interfaces;
using System.Net;
using System.Net.Http.Json;

namespace HomeNet.Services.Shared.Services.KeyVault
{
    public class KeyVaultServiceClient : IKeyVaultServiceClient
    {
        private readonly HttpClient _httpClient;

        public KeyVaultServiceClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<string?> GetSecretValueAsync(GetSecretDto secret)
        {
            var response = await _httpClient.GetAsync($"api/KeyVault/secret-value/{secret.ResourceType}/{secret.KeyType}");

            if (response.StatusCode == HttpStatusCode.NotFound)
            {
                return null;
            }
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }

        public async Task<bool> SetSecretAsync(CreateSecretDto createSecretDto)
        {
            var jsonContent = JsonContent.Create(createSecretDto);
            var response = await _httpClient.PostAsync("api/KeyVault/secret", jsonContent);
            return response.IsSuccessStatusCode;
        }
    }
}
