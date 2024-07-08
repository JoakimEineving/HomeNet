using Azure.Security.KeyVault.Secrets;
using HomeNet.Core.Dtos.KeyVault;

namespace HomeNet.Services.KeyVaultService.Services.Interfaces
{
    public interface IKeyVaultService
    {
        Task<KeyVaultSecret> GetSecretAsync(string resourceType, string keyType);
        string GetSecretName(string resourceType, string keyType);
        Task<string> GetSecretValueAsync(string resourceType, string keyType);
        Task SetSecretAsync(CreateSecretDto secret);
    }
}