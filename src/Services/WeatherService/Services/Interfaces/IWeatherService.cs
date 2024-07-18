namespace HomeNet.Services.WeatherService.Services.Interfaces
{
    public interface IWeatherService
    {
        Task<string> GetWeatherAsync(double latitude, double longitude);
    }
}