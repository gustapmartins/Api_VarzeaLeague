	# Base Image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80

ENV ASPNETCORE_URLS=http://+:80;
ENV ASPNETCORE_ENVIRONMENT=Development

# Build Image
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["VarzeaTeam.Application/VarzeaLeague.Application.csproj", "VarzeaTeam.Application/"]
COPY ["VarzeamTeam.Infra.CrossCutting/VarzeamLeague.Infra.CrossCutting.csproj", "VarzeamTeam.Infra.CrossCutting/"]
COPY ["VarzeaTeam.Domain/VarzeaLeague.Domain.csproj", "VarzeaTeam.Domain/"]
COPY ["VarzeaTeam.Infra.Data/VarzeaLeague.Infra.Data.csproj", "VarzeaTeam.Infra.Data/"]
COPY ["VarzeaTeam.Test/VarzeaLeague.Test.csproj", "VarzeaTeam.Test/"]
RUN dotnet restore "VarzeaTeam.Application/VarzeaLeague.Application.csproj"
COPY . .
WORKDIR "/src/VarzeaTeam.Application"

# Build Image
RUN dotnet restore "VarzeaTeam.Application/VarzeaLeague.Application.csproj"
COPY . .
WORKDIR "/src/VarzeaTeam.Application"
RUN dotnet build "VarzeaLeague.Application.csproj" -c Release -o /app/build

# Test Image
FROM build AS test
WORKDIR /src
COPY . .
RUN dotnet test "VarzeaTeam.Test/VarzeaLeague.Test.csproj" --logger:trx || true

# Defina o comando de execução padrão
CMD ["dotnet", "watch", "run", "--project", "/src/VarzeaTeam.Application/VarzeaLeague.Application.csproj", "--urls", "http://+:443;http://*:80"]
