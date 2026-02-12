FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app 
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["NukeAuthentication/NukeAuthentication.csproj", "NukeAuthentication/"]
RUN dotnet restore "NukeAuthentication/NukeAuthentication.csproj"

COPY . .
RUN dotnet build "NukeAuthentication//NukeAuthentication.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "NukeAuthentication//NukeAuthentication.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "NukeAuthentication.dll"]
