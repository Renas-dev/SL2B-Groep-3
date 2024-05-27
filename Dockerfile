# Use the .NET SDK image with multi-platform support
FROM --platform=$BUILDPLATFORM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG TARGETARCH
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Copy and build Dierentuin-App
COPY ["Dierentuin-App/Dierentuin-App.csproj", "Dierentuin-App/"]
RUN dotnet restore -a $TARGETARCH "Dierentuin-App/Dierentuin-App.csproj"
COPY Dierentuin-App/ Dierentuin-App/
WORKDIR "/src/Dierentuin-App"
RUN dotnet build "Dierentuin-App.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Switch back to /src
WORKDIR /src

# Copy and test Dierentuin-unit-test
COPY ["Dierentuin-unit-test/Dierentuin-unit-test.csproj", "Dierentuin-unit-test/"]
RUN dotnet restore -a $TARGETARCH "Dierentuin-unit-test/Dierentuin-unit-test.csproj"
COPY Dierentuin-unit-test/ Dierentuin-unit-test/
WORKDIR "/src/Dierentuin-unit-test"
RUN dotnet test --logger:trx

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
WORKDIR "/src/Dierentuin-App"
RUN dotnet publish "Dierentuin-App.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Final stage
FROM --platform=$TARGETARCH mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=publish /app/publish .

# Change to a non-root user
USER appuser

# Update the application to listen on port 8080
ENV ASPNETCORE_URLS=http://+:8080

ENTRYPOINT ["dotnet", "Dierentuin-App.dll"]
