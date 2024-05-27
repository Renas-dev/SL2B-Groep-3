# Use a multi-platform base image for the runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80

# Use a multi-platform base image for the build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Copy and build Dierentuin-App
COPY ["Dierentuin-App/Dierentuin-App.csproj", "Dierentuin-App/"]
RUN dotnet restore "Dierentuin-App/Dierentuin-App.csproj" --verbosity detailed
COPY Dierentuin-App/ Dierentuin-App/
WORKDIR "/src/Dierentuin-App"
RUN dotnet build "Dierentuin-App.csproj" -c $BUILD_CONFIGURATION --no-restore -o /app/build

# Switch back to /src
WORKDIR /src

# Copy and test Dierentuin-unit-test
COPY ["Dierentuin-unit-test/Dierentuin-unit-test.csproj", "Dierentuin-unit-test/"]
RUN dotnet restore "Dierentuin-unit-test/Dierentuin-unit-test.csproj" --verbosity detailed
COPY Dierentuin-unit-test/ Dierentuin-unit-test/
WORKDIR "/src/Dierentuin-unit-test"
RUN dotnet test --logger:trx

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
WORKDIR "/src/Dierentuin-App"
RUN dotnet publish "Dierentuin-App.csproj" -c $BUILD_CONFIGURATION --no-restore -o /app/publish /p:UseAppHost=false

# Final stage
FROM base AS final
WORKDIR /var/www/html
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Dierentuin-App.dll"]
