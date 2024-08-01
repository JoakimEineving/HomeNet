
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY ["Services/LightingService/Api/HomeNet.Services.LightingService.Api.csproj", "Services/LightingService/Api/"]
COPY ["Services/LightingService/Services/HomeNet.Services.LightingService.Services.csproj", "Services/LightingService/Services/"]
COPY ["Services/Shared/Services/HomeNet.Services.Shared.Services.csproj", "Services/Shared/Services/"]
COPY ["Core/HomeNet.Core.csproj", "Core/"]

RUN dotnet restore "Services/LightingService/Api/HomeNet.Services.LightingService.Api.csproj"

COPY . .
WORKDIR "/src/Services/LightingService/Api"
RUN dotnet build "HomeNet.Services.LightingService.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "HomeNet.Services.LightingService.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "HomeNet.Services.LightingService.Api.dll"]
