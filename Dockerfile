# Use the appropriate base image for ARM64
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base-arm64
WORKDIR /app
EXPOSE 8080

# Use the appropriate base image for x64
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base-x64
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Copy and build Dierentuin-App
COPY ["Dierentuin-App/Dierentuin-App.csproj", "Dierentuin-App/"]
RUN dotnet restore "Dierentuin-App/Dierentuin-App.csproj"
COPY Dierentuin-App/ Dierentuin-App/
WORKDIR "/src/Dierentuin-App"
RUN dotnet build "Dierentuin-App.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Switch back to /src
WORKDIR /src

# Copy and test Dierentuin-unit-test
COPY ["Dierentuin-unit-test/Dierentuin-unit-test.csproj", "Dierentuin-unit-test/"]
RUN dotnet restore "Dierentuin-unit-test/Dierentuin-unit-test.csproj"
COPY Dierentuin-unit-test/ Dierentuin-unit-test/
WORKDIR "/src/Dierentuin-unit-test"
RUN dotnet test --logger:trx

FROM build AS publish-arm64
WORKDIR "/src/Dierentuin-App"
RUN dotnet publish "Dierentuin-App.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM build AS publish-x64
WORKDIR "/src/Dierentuin-App"
RUN dotnet publish "Dierentuin-App.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Combine ARM64 and x64 publish stages
FROM base-arm64 AS final-arm64
WORKDIR /app
COPY --from=publish-arm64 /app/publish .
ENTRYPOINT ["dotnet", "Dierentuin-App.dll"]

FROM base-x64 AS final-x64
WORKDIR /app
COPY --from=publish-x64 /app/publish .
ENTRYPOINT ["dotnet", "Dierentuin-App.dll"]
