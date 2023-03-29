FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["VehicleGenius.Api/VehicleGenius.Api.csproj", "VehicleGenius.Api/"]
RUN dotnet restore "VehicleGenius.Api/VehicleGenius.Api.csproj"
COPY . .
WORKDIR "/src/VehicleGenius.Api"
RUN dotnet build "VehicleGenius.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "VehicleGenius.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "VehicleGenius.Api.dll"]
