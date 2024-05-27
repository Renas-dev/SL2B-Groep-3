# Use a multi-platform base image for the build
FROM --platform=$TARGETPLATFORM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Print diagnostic information
RUN uname -a
RUN dotnet --info
RUN apt-get update && apt-get install -y libicu-dev

# Copy and restore the project
COPY ["Dierentuin-App/Dierentuin-App.csproj", "Dierentuin-App/"]
RUN dotnet restore "Dierentuin-App/Dierentuin-App.csproj" --verbosity detailed

# Copy the remaining files and build the project
COPY Dierentuin-App/ Dierentuin-App/
WORKDIR "/src/Dierentuin-App"
RUN dotnet build "Dierentuin-App.csproj" -c Release -o /app/build --verbosity detailed

# Final stage
FROM --platform=$TARGETPLATFORM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
COPY --from=build /app/build .
ENTRYPOINT ["dotnet", "Dierentuin-App.dll"]
