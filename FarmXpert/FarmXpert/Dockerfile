FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["FarmXpert/FarmXpert/FarmXpert.csproj", "FarmXpert/FarmXpert/"]
COPY ["FarmXpert/FarmXpert.Client/FarmXpert.Client.csproj", "FarmXpert/FarmXpert.Client/"]
RUN dotnet restore "FarmXpert/FarmXpert/FarmXpert.csproj"
COPY . .
WORKDIR "/src/FarmXpert/FarmXpert"
RUN dotnet build "./FarmXpert.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./FarmXpert.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "FarmXpert.dll"]
