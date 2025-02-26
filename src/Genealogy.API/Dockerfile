FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Genealogy.API/Genealogy.API.csproj", "Genealogy.API/"]
COPY ["Genealogy.Infrastructure/Genealogy.Infrastructure.csproj", "Genealogy.Infrastructure/"]
COPY ["Genealogy.Domain/Genealogy.Domain.csproj", "Genealogy.Domain/"]
RUN dotnet restore "Genealogy.API/Genealogy.API.csproj"
COPY . .
WORKDIR "/src/Genealogy.API"
RUN dotnet build "Genealogy.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "Genealogy.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Genealogy.API.dll"]
