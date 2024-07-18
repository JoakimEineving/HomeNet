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
    client.BaseAddress = new Uri("http://localhost:5110");
});
builder.Services.AddScoped<IWeatherService, WeatherService>();

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
