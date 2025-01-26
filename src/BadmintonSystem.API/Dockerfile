FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/BadmintonSystem.API/BadmintonSystem.API.csproj", "src/BadmintonSystem.API/"]
COPY ["src/BadmintonSystem.Application/BadmintonSystem.Application.csproj", "src/BadmintonSystem.Application/"]
COPY ["src/BadmintonSystem.Contract/BadmintonSystem.Contract.csproj", "src/BadmintonSystem.Contract/"]
COPY ["src/BadmintonSystem.Domain/BadmintonSystem.Domain.csproj", "src/BadmintonSystem.Domain/"]
COPY ["src/BadmintonSystem.Persistence/BadmintonSystem.Persistence.csproj", "src/BadmintonSystem.Persistence/"]
COPY ["src/BadmintonSystem.Infrastructure.Dapper/BadmintonSystem.Infrastructure.Dapper.csproj", "src/BadmintonSystem.Infrastructure.Dapper/"]
COPY ["src/BadmintonSystem.Infrastructure/BadmintonSystem.Infrastructure.csproj", "src/BadmintonSystem.Infrastructure/"]
COPY ["src/BadmintonSystem.Presentation/BadmintonSystem.Presentation.csproj", "src/BadmintonSystem.Presentation/"]
RUN dotnet restore "src/BadmintonSystem.API/BadmintonSystem.API.csproj"
COPY . .
WORKDIR "/src/src/BadmintonSystem.API"
RUN dotnet build "BadmintonSystem.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "BadmintonSystem.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "BadmintonSystem.API.dll"]
