using HomeNet.Services.LightingService.Services.Interfaces;
using HomeNet.Services.LightingService.Services.PhilipsHue;
using HomeNet.Services.Shared.Services.Interfaces;
using HomeNet.Services.Shared.Services.KeyVault;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddControllers();
builder.Services.AddSwaggerGen();

builder.Services.AddHttpClient<IKeyVaultServiceClient, KeyVaultServiceClient>(client =>
{
    client.BaseAddress = new Uri("http://keyvaultservice:8080");
    //client.BaseAddress = new Uri("https://localhost:7299/");
});

builder.Services.AddHttpClient<IHueService, HueService>(client =>
{
    client.BaseAddress = new Uri("https://api.meethue.com");
});
builder.Services.AddHttpClient<IHueTokenService, HueTokenService>(client =>
{
    client.BaseAddress = new Uri("https://api.meethue.com");
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
