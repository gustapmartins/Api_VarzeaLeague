# Base Image
FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 8080

ENV ASPNETCORE_URLS=http://+:8000;http://+:80;
ENV ASPNETCORE_ENVIRONMENT=Development

# Build Image
FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["VarzeaTeam.Application/VarzeaLeague.Application.csproj", "VarzeaTeam.Application/"]
COPY ["VarzeamTeam.Infra.CrossCutting/VarzeamLeague.Infra.CrossCutting.csproj", "VarzeamTeam.Infra.CrossCutting/"]
COPY ["VarzeaTeam.Domain/VarzeaLeague.Domain.csproj", "VarzeaTeam.Domain/"]
COPY ["VarzeaTeam.Infra.Data/VarzeaLeague.Infra.Data.csproj", "VarzeaTeam.Infra.Data/"]
COPY ["VarzeaTeam.Test/VarzeaLeague.Test.csproj", "VarzeaTeam.Test/"]
RUN dotnet restore "VarzeaTeam.Application/VarzeaLeague.Application.csproj"
COPY . .
WORKDIR "/src/VarzeaTeam.Application"
RUN dotnet build "VarzeaLeague.Application.csproj" -c Release -o /app/build

# Test Image
FROM build AS test
WORKDIR /src
COPY . .
RUN dotnet test "VarzeaTeam.Test/VarzeaLeague.Test.csproj" --logger:trx || true

# Publish Image
FROM build AS publish
RUN dotnet publish "VarzeaLeague.Application.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Final Image
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "VarzeaLeague.Application.dll"]
