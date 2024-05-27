# Use the latest stable version of the multi-platform base image for the runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80

# Use the latest stable version of the multi-platform base image for the build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Display .NET SDK info and environment variables for debugging
RUN dotnet --info
RUN env

# Copy and build Dierentuin-App
COPY ["Dierentuin-App/Dierentuin-App.csproj", "Dierentuin-App/"]
# Show the content of the csproj file for debugging
RUN cat Dierentuin-App/Dierentuin-App.csproj

# Attempt to restore with detailed verbosity
RUN dotnet restore "Dierentuin-App/Dierentuin-App.csproj" --verbosity diagnostic

# If the restore is successful, proceed
COPY Dierentuin-App/ Dierentuin-App/
WORKDIR "/src/Dierentuin-App"
RUN dotnet build "Dierentuin-App.csproj" -c $BUILD_CONFIGURATION --verbosity diagnostic -o /app/build

# Publish the application
RUN dotnet publish "Dierentuin-App.csproj" -c $BUILD_CONFIGURATION --verbosity diagnostic -o /app/publish

# Final stage
FROM base AS final
WORKDIR /var/www/html
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "Dierentuin-App.dll"]
