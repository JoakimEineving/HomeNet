
namespace HomeNet.Services.LightingService.Services.Interfaces
{
    public interface IHueService
    {
        Task<string> GetLightsAsync();
        Task<string> SetLightStateAsync(string lightNumber, bool isOn);
    }
}