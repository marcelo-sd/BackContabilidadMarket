# Fase base
FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80

# Fase de construcción
FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["ContabilidadMarket.csproj", "ContabilidadMarket/"]
#COPY ["../WorkerService0001/*.csproj", "WorkerService0001/"]
#COPY ["../WorkerService0001/WorkerService0001.csproj", "WorkerService0001/"]


RUN dotnet restore "ContabilidadMarket.csproj"
COPY . .
WORKDIR "/src/ContabilidadMarket"
RUN dotnet build "ContabilidadMarket.csproj" -c Release -o /app/build

# Fase de publicación
FROM build AS publish
RUN dotnet publish "ContabilidadMarket.csproj" -c Release -o /app/publish

# Fase final
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ContabilidadMarket.dll"]
