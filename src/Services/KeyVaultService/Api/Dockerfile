
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY ["Services/KeyVaultService/Api/HomeNet.Services.KeyVaultService.Api.csproj", "Services/KeyVaultService/Api/"]
COPY ["Services/KeyVaultService/Services/HomeNet.Services.KeyVaultService.Services.csproj", "Services/KeyVaultService/Services/"]
COPY ["Core/HomeNet.Core.csproj", "Core/"]

RUN dotnet restore "Services/KeyVaultService/Api/HomeNet.Services.KeyVaultService.Api.csproj"

COPY . .
WORKDIR "/src/Services/KeyVaultService/Api"
RUN dotnet build "HomeNet.Services.KeyVaultService.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "HomeNet.Services.KeyVaultService.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "HomeNet.Services.KeyVaultService.Api.dll"]
