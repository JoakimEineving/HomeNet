using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeNet.Services.LightingService.Services.Models
{
    public class RefreshAccessTokenCredentials
    {
        public string ClientId { get; set; } = string.Empty;
        public string ClientSecret { get; set; } = string.Empty;
        public string RefreshToken { get; set; } = string.Empty;

    }
}
