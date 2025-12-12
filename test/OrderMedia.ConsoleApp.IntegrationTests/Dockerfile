FROM mcr.microsoft.com/dotnet/sdk:10.0

ARG RESOURCE_REAPER_SESSION_ID="00000000-0000-0000-0000-000000000000"
LABEL "org.testcontainers.resource-reaper-session"=$RESOURCE_REAPER_SESSION_ID

# Set build configuration
ARG BUILD_CONFIGURATION=Release

# Set working directory
WORKDIR /app

# Copy and restore dependencies
COPY ["src/OrderMedia.ConsoleApp/OrderMedia.ConsoleApp.csproj", "src/OrderMedia.ConsoleApp/"]
COPY ["src/OrderMedia/OrderMedia.csproj", "src/OrderMedia/"]
RUN dotnet restore "src/OrderMedia.ConsoleApp/OrderMedia.ConsoleApp.csproj"

# Copy the rest of the source code
COPY . .

# Build the application
WORKDIR /app/src/OrderMedia.ConsoleApp
RUN dotnet build "OrderMedia.ConsoleApp.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Publish the application
RUN dotnet publish "OrderMedia.ConsoleApp.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Switch back to root for any file permission adjustments
USER root
WORKDIR /app/publish
RUN chmod 744 appsettings.json

# Set the entry point
ENTRYPOINT ["dotnet", "OrderMedia.ConsoleApp.dll"]
