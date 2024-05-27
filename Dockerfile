# Use a multi-platform base image for the runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80

# Use a multi-platform base image for the build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Set environment variables
ENV DOTNET_CLI_TELEMETRY_OPTOUT=1
ENV DOTNET_SKIP_FIRST_TIME_EXPERIENCE=1
ENV DOTNET_NOLOGO=1

# Copy and build Dierentuin-App
COPY ["Dierentuin-App/Dierentuin-App.csproj", "Dierentuin-App/"]
RUN dotnet nuget locals all --clear
RUN dotnet restore "Dierentuin-App/Dierentuin-App.csproj" --disable-parallel

# Copy the rest of the files and build
COPY Dierentuin-App/ Dierentuin-App/
WORKDIR "/src/Dierentuin-App"
RUN echo "Starting build process" && dotnet build "Dierentuin-App.csproj" -c $BUILD_CONFIGURATION -o /app/build && echo "Build process completed"

# Switch back to /src
WORKDIR /src

# Copy and test Dierentuin-unit-test
COPY ["Dierentuin-unit-test/Dierentuin-unit-test.csproj", "Dierentuin-unit-test/"]
RUN dotnet restore "Dierentuin-unit-test/Dierentuin-unit-test.csproj" --disable-parallel
COPY Dierentuin-unit-test/ Dierentuin-unit-test/
WORKDIR "/src/Dierentuin-unit-test"
RUN echo "Starting test process" && dotnet test --logger:trx && echo "Test process completed"

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
WORKDIR "/src/Dierentuin-App"
RUN dotnet publish "Dierentuin-App.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Final stage
FROM base AS final
WORKDIR /var/www/html
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Dierentuin-App.dll"]
