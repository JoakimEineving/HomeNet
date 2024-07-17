using HomeNet.Core.Dtos.KeyVault;

namespace HomeNet.Services.Shared.Services.Interfaces
{
    public interface IKeyVaultServiceClient
    {
        Task<string> GetSecretValueAsync(GetSecretDto secret);
    }
}