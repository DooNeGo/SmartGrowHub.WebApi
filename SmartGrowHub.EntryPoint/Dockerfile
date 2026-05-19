FROM mcr.microsoft.com/dotnet/runtime:9.0 AS base
USER $APP_UID
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["SmartGrowHub.EntryPoint/SmartGrowHub.EntryPoint.csproj", "SmartGrowHub.EntryPoint/"]
RUN dotnet restore "SmartGrowHub.EntryPoint/SmartGrowHub.EntryPoint.csproj"
COPY . .
WORKDIR "/src/SmartGrowHub.EntryPoint"
RUN dotnet build "SmartGrowHub.EntryPoint.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "SmartGrowHub.EntryPoint.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SmartGrowHub.EntryPoint.dll"]
