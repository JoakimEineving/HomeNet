using Azure;
using Azure.Security.KeyVault.Secrets;

namespace HomeNet.Services.KeyVaultService.Services.Interfaces
{
    public interface IAzureSecretClient
    {
        Task<KeyVaultSecret> GetSecretAsync(string secretName);
        Task<Response<KeyVaultSecret>> SetSecretAsync(KeyVaultSecret secret);
    }
}