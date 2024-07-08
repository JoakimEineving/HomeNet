
namespace HomeNet.Core.Dtos.KeyVault
{
    public class CreateSecretDto
    {
        public string Value { get; set; } = string.Empty;
        public string ResourceType { get; set; } = string.Empty;
        public string KeyType { get; set; } = string.Empty;
        public DateTimeOffset ExpiresOn { get; set; }

        public bool IsValid()
        {
            return !string.IsNullOrWhiteSpace(Value) && !string.IsNullOrWhiteSpace(ResourceType) && !string.IsNullOrWhiteSpace(KeyType) && ExpiresOn > DateTimeOffset.UtcNow;
        }
    }
}
