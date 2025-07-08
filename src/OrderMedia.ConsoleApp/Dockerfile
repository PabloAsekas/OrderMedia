FROM mcr.microsoft.com/dotnet/runtime:9.0 AS base
USER $APP_UID
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/OrderMedia.ConsoleApp/OrderMedia.ConsoleApp.csproj", "src/OrderMedia.ConsoleApp/"]
COPY ["src/OrderMedia/OrderMedia.csproj", "src/OrderMedia/"]
RUN dotnet restore "src/OrderMedia.ConsoleApp/OrderMedia.ConsoleApp.csproj"
COPY . .
WORKDIR "/src/src/OrderMedia.ConsoleApp"
RUN dotnet build "OrderMedia.ConsoleApp.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "OrderMedia.ConsoleApp.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "OrderMedia.ConsoleApp.dll"]
