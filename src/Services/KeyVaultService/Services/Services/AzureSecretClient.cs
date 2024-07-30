using Azure;
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using HomeNet.Services.KeyVaultService.Services.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace HomeNet.Services.KeyVaultService.Services.Services
{
    public class AzureSecretClient : IAzureSecretClient
    {
        private readonly SecretClient _secretClient;
        private readonly ILogger<AzureSecretClient> _logger;

        public AzureSecretClient(IConfiguration configuration, ILogger<AzureSecretClient> logger)
        {
            _logger = logger;
            try
            {
                var vaultUri = new Uri(configuration["KeyVaultUri"] ?? throw new ArgumentNullException("KeyVaultUri is missing from configuration"));
                _secretClient = new SecretClient(vaultUri, new DefaultAzureCredential());
                _logger.LogInformation("SecretClient successfully initialized.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to initialize SecretClient.");
                throw;
            }
        }

        public async Task<KeyVaultSecret> GetSecretAsync(string secretName)
        {
            try
            {
                return await _secretClient.GetSecretAsync(secretName);
            }
            catch (RequestFailedException ex) when (ex.Status == 404)
            {
                _logger.LogWarning("Secret {secretName} not found in KeyVault", secretName);
                throw new KeyNotFoundException();
            }
        }

        public async Task<Response<KeyVaultSecret>> SetSecretAsync(KeyVaultSecret secret)
        {
            return await _secretClient.SetSecretAsync(secret);
        }
    }
}
