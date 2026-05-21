# Базовый образ для рантайма
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base

USER root
RUN apt-get update && \
    apt-get install -y libgssapi-krb5-2 && \
    rm -rf /var/lib/apt/lists/* \
    
USER $APP_UID
WORKDIR /app
EXPOSE 8080

# Этап сборки
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

COPY ["SmartGrowHub.AspNetCore/SmartGrowHub.AspNetCore.csproj", "SmartGrowHub.AspNetCore/"]
COPY ["SmartGrowHub.Infrastructure/SmartGrowHub.Infrastructure.csproj", "SmartGrowHub.Infrastructure/"]
COPY ["SmartGrowHub.Shared/SmartGrowHub.Shared.csproj", "SmartGrowHub.Shared/"]
COPY ["SmartGrowHub.Application/SmartGrowHub.Application.csproj", "SmartGrowHub.Application/"]
COPY ["SmartGrowHub.Domain/SmartGrowHub.Domain.csproj", "SmartGrowHub.Domain/"]

RUN dotnet restore "SmartGrowHub.AspNetCore/SmartGrowHub.AspNetCore.csproj"

COPY . .
WORKDIR "/src/SmartGrowHub.AspNetCore"
RUN dotnet build "SmartGrowHub.AspNetCore.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "SmartGrowHub.AspNetCore.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SmartGrowHub.AspNetCore.dll"]