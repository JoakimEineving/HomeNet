using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeNet.Core.Dtos.KeyVault
{
    public class GetSecretDto
    {
        public string ResourceType { get; set; } = string.Empty;
        public string KeyType { get; set; } = string.Empty;

        public bool IsValid()
        {
            return !string.IsNullOrWhiteSpace(ResourceType) && !string.IsNullOrWhiteSpace(KeyType);
        }
    }
}
