# Use a multi-platform base image for the runtime
FROM --platform=$TARGETPLATFORM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80

# Use a multi-platform base image for the build
FROM --platform=$TARGETPLATFORM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Display .NET SDK info and environment variables for debugging
RUN dotnet --info
RUN env

# Copy the NuGet.config file to the container
COPY NuGet.config ./

# Copy the project file and restore as distinct layers
COPY ["Dierentuin-App/Dierentuin-App.csproj", "Dierentuin-App/"]
# Show the content of the csproj file for debugging
RUN cat Dierentuin-App/Dierentuin-App.csproj

# Attempt to restore with detailed verbosity using the NuGet.config file
RUN dotnet restore "Dierentuin-App/Dierentuin-App.csproj" --configfile ./NuGet.config --verbosity diagnostic

# If the restore is successful, proceed
COPY Dierentuin-App/ Dierentuin-App/
WORKDIR "/src/Dierentuin-App"
RUN dotnet build "Dierentuin-App.csproj" -c $BUILD_CONFIGURATION --verbosity diagnostic -o /app/build

# Switch back to /src
WORKDIR /src

# Copy and test Dierentuin-unit-test
COPY ["Dierentuin-unit-test/Dierentuin-unit-test.csproj", "Dierentuin-unit-test/"]
RUN dotnet restore "Dierentuin-unit-test/Dierentuin-unit-test.csproj" --configfile ./NuGet.config --verbosity diagnostic
COPY Dierentuin-unit-test/ Dierentuin-unit-test/
WORKDIR "/src/Dierentuin-unit-test"
RUN dotnet test --logger:trx

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
WORKDIR "/src/Dierentuin-App"
RUN dotnet publish "Dierentuin-App.csproj" -c $BUILD_CONFIGURATION --verbosity diagnostic -o /app/publish /p:UseAppHost=false

# Final stage
FROM base AS final
WORKDIR /var/www/html
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Dierentuin-App.dll"]
