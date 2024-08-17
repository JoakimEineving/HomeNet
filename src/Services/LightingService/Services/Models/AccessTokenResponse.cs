
using System.Text.Json.Serialization;

namespace HomeNet.Services.LightingService.Services.Models
{
    public class AccessTokenResponse
    {

        [JsonPropertyName("access_token")]
        public string AccessToken { get; set; } = null!;

        [JsonPropertyName("expires_in")]
        public int ExpiresIn { get; set; }

        [JsonPropertyName("refresh_token")]
        public string RefreshToken { get; set; } = null!;

        [JsonPropertyName("token_type")]
        public string TokenType { get; set; } = null!;

        [JsonPropertyName("access_token_expires_in")]
        public string AccessTokenExpiresIn { get; set; } = null!;

        [JsonPropertyName("refresh_token_expires_in")]
        public string RefreshTokenExpiresIn { get; set; } = null!;

        public bool IsValid()
        {
            return !string.IsNullOrEmpty(AccessToken) && !string.IsNullOrEmpty(RefreshToken) && !string.IsNullOrEmpty(AccessTokenExpiresIn) && !string.IsNullOrEmpty(RefreshTokenExpiresIn);
        }
    }
}
