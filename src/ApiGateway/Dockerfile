FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["ApiGateway/HomeNet.ApiGateway.csproj", "ApiGateway/"]
RUN dotnet restore "ApiGateway/HomeNet.ApiGateway.csproj"
COPY . .
WORKDIR "/src/ApiGateway"
RUN dotnet build "HomeNet.ApiGateway.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "HomeNet.ApiGateway.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "HomeNet.ApiGateway.dll"]
