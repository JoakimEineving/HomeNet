# Use the official ASP.NET Core runtime as a parent image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80

# Use the SDK image to build the application
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy the csproj files and restore dependencies
COPY ["Services/WeatherService/Api/HomeNet.Services.WeatherService.Api.csproj", "Services/WeatherService/Api/"]
COPY ["Services/WeatherService/Services/HomeNet.Services.WeatherService.Services.csproj", "Services/WeatherService/Services/"]
COPY ["Services/Shared/Services/HomeNet.Services.Shared.Services.csproj", "Services/Shared/Services/"]

RUN dotnet restore "Services/WeatherService/Api/HomeNet.Services.WeatherService.Api.csproj"

# Copy the entire solution and build
COPY . .
WORKDIR "/src/Services/WeatherService/Api"
RUN dotnet build "HomeNet.Services.WeatherService.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "HomeNet.Services.WeatherService.Api.csproj" -c Release -o /app/publish

# Final stage/image
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "HomeNet.Services.WeatherService.Api.dll"]
