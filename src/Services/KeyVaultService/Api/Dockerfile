# Use the official ASP.NET Core runtime as a parent image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80

# Use the SDK image to build the application
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy the csproj files and restore dependencies
COPY ["Services/KeyVaultService/Api/HomeNet.Services.KeyVaultService.Api.csproj", "Services/KeyVaultService/Api/"]
COPY ["Services/KeyVaultService/Services/HomeNet.Services.KeyVaultService.Services.csproj", "Services/KeyVaultService/Services/"]
COPY ["Core/HomeNet.Core.csproj", "Core/"]

RUN dotnet restore "Services/KeyVaultService/Api/HomeNet.Services.KeyVaultService.Api.csproj"

# Copy the entire solution and build
COPY . .
WORKDIR "/src/Services/KeyVaultService/Api"
RUN dotnet build "HomeNet.Services.KeyVaultService.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "HomeNet.Services.KeyVaultService.Api.csproj" -c Release -o /app/publish

# Final stage/image
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "HomeNet.Services.KeyVaultService.Api.dll"]
