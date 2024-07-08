using HomeNet.Services.KeyVaultService.Services.Interfaces;
using HomeNet.Services.KeyVaultService.Services.Services;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddControllers();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "KeyVault API", Version = "v1" });
});

builder.Services.AddSingleton<IAzureSecretClient, AzureSecretClient>();
builder.Services.AddScoped<IKeyVaultService, KeyVaultService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "KeyVault API v1"));
}

app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthorization();

app.MapControllers();


app.Run();
