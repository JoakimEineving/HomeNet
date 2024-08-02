using HomeNet.Services.Shared.Services.Interfaces;
using HomeNet.Services.Shared.Services.KeyVault;
using HomeNet.Services.WeatherService.Services.Interfaces;
using HomeNet.Services.WeatherService.Services.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddControllers();
builder.Services.AddSwaggerGen();

builder.Services.AddHttpClient<IKeyVaultServiceClient, KeyVaultServiceClient>(client =>
{
    client.BaseAddress = new Uri("http://keyvaultservice:8080");
});
builder.Services.AddHttpClient<IWeatherService, WeatherService>(client =>
{
    client.BaseAddress = new Uri("https://api.openweathermap.org/data/3.0/onecall");
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthorization();

app.MapControllers();

app.Run();
