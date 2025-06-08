FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/Genealogy.Presentation/Genealogy.Presentation.csproj", "src/Genealogy.Presentation/"]
RUN dotnet restore "src/Genealogy.Presentation/Genealogy.Presentation.csproj"
COPY . .
WORKDIR "/src/src/Genealogy.Presentation"
RUN dotnet build "./Genealogy.Presentation.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Genealogy.Presentation.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Genealogy.Presentation.dll"]
