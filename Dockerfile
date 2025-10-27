# --- Estágio 1: Build (Compilação) ---
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY src/RastreamentoCargas.API/RastreamentoCargas.API.csproj src/RastreamentoCargas.API/
COPY src/RastreamentoCargas.Application/RastreamentoCargas.Application.csproj src/RastreamentoCargas.Application/
COPY src/RastreamentoCargas.Domain/RastreamentoCargas.Domain.csproj src/RastreamentoCargas.Domain/
COPY src/RastreamentoCargas.Infrastructure/RastreamentoCargas.Infrastructure.csproj src/RastreamentoCargas.Infrastructure/

RUN dotnet restore src/RastreamentoCargas.API/RastreamentoCargas.API.csproj

COPY . .

WORKDIR "/src/src/RastreamentoCargas.API"
RUN dotnet publish "RastreamentoCargas.API.csproj" -c Release -o /app/publish

# --- Estágio 2: Final (Execução) ---
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "RastreamentoCargas.API.dll"]