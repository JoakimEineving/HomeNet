using HomeNet.Core.Dtos.KeyVault;
using HomeNet.Services.KeyVaultService.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace HomeNet.Services.KeyVaultService.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class KeyVaultController : ControllerBase
    {
        private readonly ILogger<KeyVaultController> _logger;
        private readonly IKeyVaultService _keyVaultService;

        public KeyVaultController(ILogger<KeyVaultController> logger, IKeyVaultService keyVaultService)
        {
            _logger = logger;
            _keyVaultService = keyVaultService;
        }

        [HttpGet("secret-value/{resourceType}/{keyType}")]
        public async Task<IActionResult> GetSecretValueAsync(string resourceType, string keyType)
        {
            try
            {
                var secretValue = await _keyVaultService.GetSecretValueAsync(resourceType, keyType);
                return Ok(secretValue);
            }
            catch (KeyNotFoundException)
            {
                _logger.LogWarning("Secret not found or has expired for resourceType: {resourceType}, keyType: {keyType}", resourceType, keyType);
                return NotFound("Secret not found or has expired.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving secret value for resourceType: {resourceType}, keyType: {keyType}", resourceType, keyType);
                return StatusCode(500, "Internal server error.");
            }
        }

        [HttpGet("secret/{resourceType}/{keyType}")]
        public async Task<IActionResult> GetSecretAsync(string resourceType, string keyType)
        {
            try
            {
                var secret = await _keyVaultService.GetSecretAsync(resourceType, keyType);
                return Ok(secret);
            }
            catch (KeyNotFoundException)
            {
                _logger.LogWarning("Secret not found or has expired for resourceType: {resourceType}, keyType: {keyType}", resourceType, keyType);
                return NotFound("Secret not found or has expired.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving secret for resourceType: {resourceType}, keyType: {keyType}", resourceType, keyType);
                return StatusCode(500, "Internal server error.");
            }
        }

        [HttpPost("secret")]
        public async Task<IActionResult> SetSecretAsync([FromBody] CreateSecretDto secret)
        {
            try
            {
                await _keyVaultService.SetSecretAsync(secret);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error setting secret for resourceType: {resourceType}, keyType: {keyType}", secret.ResourceType, secret.KeyType);
                return StatusCode(500, "Internal server error.");
            }
        }
    }
}
