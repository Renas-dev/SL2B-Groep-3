# Use the .NET SDK image with multi-platform support
FROM --platform=$BUILDPLATFORM mcr.microsoft.com/dotnet/sdk:8.0-preview AS build
ARG TARGETARCH
WORKDIR /source

# Copy csproj and restore as distinct layers
COPY ["Dierentuin-App/Dierentuin-App.csproj", "Dierentuin-App/"]
RUN dotnet restore -a $TARGETARCH "Dierentuin-App/Dierentuin-App.csproj"

# Copy everything else and build the app
COPY . .
RUN dotnet publish -a $TARGETARCH --no-restore -o /app

# Final stage/image
FROM --platform=$TARGETARCH mcr.microsoft.com/dotnet/aspnet:8.0-preview
WORKDIR /app
COPY --from=build /app .

# Set user
USER $APP_UID
ENTRYPOINT ["dotnet", "Dierentuin-App.dll"]
