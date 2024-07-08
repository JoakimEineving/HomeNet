using Azure.Security.KeyVault.Secrets;
using HomeNet.Core.Dtos.KeyVault;
using HomeNet.Services.KeyVaultService.Services.Interfaces;
using Microsoft.Extensions.Logging;

namespace HomeNet.Services.KeyVaultService.Services.Services
{
    public class KeyVaultService : IKeyVaultService
    {
        private readonly ILogger<KeyVaultService> _logger;
        private readonly IAzureSecretClient _secretClient;

        public KeyVaultService(ILogger<KeyVaultService> logger, IAzureSecretClient secretClient)
        {
            _logger = logger;
            _secretClient = secretClient;
        }

        public async Task<string> GetSecretValueAsync(string resourceType, string keyType)
        {
            var secretName = GetSecretName(resourceType, keyType);
            KeyVaultSecret retrievedSecret = await _secretClient.GetSecretAsync(secretName);

            if (retrievedSecret.Properties.ExpiresOn < DateTimeOffset.UtcNow)
            {
                _logger.LogWarning("Secret {secretName} has expired.", secretName);
                throw new KeyNotFoundException();
            }
            return retrievedSecret.Value;
        }
        public async Task<KeyVaultSecret> GetSecretAsync(string resourceType, string keyType)
        {
            var secretName = GetSecretName(resourceType, keyType);
            KeyVaultSecret retrievedSecret = await _secretClient.GetSecretAsync(secretName);

            if (retrievedSecret.Properties.ExpiresOn < DateTimeOffset.UtcNow)
            {
                _logger.LogWarning("Secret {secretName} has expired.", secretName);
                throw new KeyNotFoundException();
            }

            return retrievedSecret;
        }

        public async Task SetSecretAsync(CreateSecretDto secret)
        {
            var secretName = GetSecretName(secret.ResourceType, secret.KeyType);
            var secretToSet = new KeyVaultSecret(secretName, secret.Value);
            secretToSet.Properties.ExpiresOn = secret.ExpiresOn;

            await _secretClient.SetSecretAsync(secretToSet);
        }

        public string GetSecretName(string resourceType, string keyType)
        {
            return $"HomeNet--{resourceType}--{keyType}";
        }
    }
}
