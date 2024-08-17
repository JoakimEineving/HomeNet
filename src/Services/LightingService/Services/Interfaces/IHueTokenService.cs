namespace HomeNet.Services.LightingService.Services.Interfaces
{
    public interface IHueTokenService
    {
        Task<string> RefreshAccessTokenAsync();
    }
}