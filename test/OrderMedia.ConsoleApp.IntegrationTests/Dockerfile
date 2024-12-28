#FROM mcr.microsoft.com/dotnet/runtime:8.0 AS base
#ARG RESOURCE_REAPER_SESSION_ID="00000000-0000-0000-0000-000000000000"
#LABEL "org.testcontainers.resource-reaper-session"=$RESOURCE_REAPER_SESSION_ID
#WORKDIR /app
#RUN chown app:app .
#USER app
#
#FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
#ARG RESOURCE_REAPER_SESSION_ID="00000000-0000-0000-0000-000000000000"
#LABEL "org.testcontainers.resource-reaper-session"=$RESOURCE_REAPER_SESSION_ID
#ARG BUILD_CONFIGURATION=Release
#WORKDIR /src
#COPY ["src/OrderMedia.ConsoleApp/OrderMedia.ConsoleApp.csproj", "src/OrderMedia.ConsoleApp/"]
#COPY ["src/OrderMedia/OrderMedia.csproj", "src/OrderMedia/"]
#RUN dotnet restore "src/OrderMedia.ConsoleApp/OrderMedia.ConsoleApp.csproj"
#COPY . .
#WORKDIR "/src/src/OrderMedia.ConsoleApp"
#RUN dotnet build "OrderMedia.ConsoleApp.csproj" -c $BUILD_CONFIGURATION -o /app/build
#
#FROM build AS publish
#ARG RESOURCE_REAPER_SESSION_ID="00000000-0000-0000-0000-000000000000"
#LABEL "org.testcontainers.resource-reaper-session"=$RESOURCE_REAPER_SESSION_ID
#ARG BUILD_CONFIGURATION=Release
#RUN dotnet publish "OrderMedia.ConsoleApp.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false
#
#FROM base AS final
#ARG RESOURCE_REAPER_SESSION_ID="00000000-0000-0000-0000-000000000000"
#LABEL "org.testcontainers.resource-reaper-session"=$RESOURCE_REAPER_SESSION_ID
#WORKDIR /app
#COPY --from=publish /app/publish .
#USER root
#RUN chmod 744 appsettings.json
#ENTRYPOINT ["dotnet", "OrderMedia.ConsoleApp.dll"]

FROM mcr.microsoft.com/dotnet/sdk:8.0

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
