# Base Image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80

ENV ASPNETCORE_URLS=http://+:80
ENV ASPNETCORE_ENVIRONMENT=Development

# Build Image
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copiando arquivos de projeto e restaurando dependências
COPY ["VarzeaTeam.Application/VarzeaLeague.Application.csproj", "VarzeaTeam.Application/"]
COPY ["VarzeamTeam.Infra.CrossCutting/VarzeamLeague.Infra.CrossCutting.csproj", "VarzeamTeam.Infra.CrossCutting/"]
COPY ["VarzeaTeam.Domain/VarzeaLeague.Domain.csproj", "VarzeaTeam.Domain/"]
COPY ["VarzeaTeam.Infra.Data/VarzeaLeague.Infra.Data.csproj", "VarzeaTeam.Infra.Data/"]
COPY ["VarzeaTeam.Test/VarzeaLeague.Test.csproj", "VarzeaTeam.Test/"]

RUN dotnet restore "VarzeaTeam.Application/VarzeaLeague.Application.csproj"

# Copiando todos os arquivos e construindo o projeto
COPY . .
WORKDIR "/src/VarzeaTeam.Application"
RUN dotnet build "VarzeaLeague.Application.csproj" -c Release -o /app/build

# Test Image
FROM build AS test
WORKDIR /src
RUN dotnet test "VarzeaTeam.Test/VarzeaLeague.Test.csproj" --collect:"Xplat Code Coverage" --results-directory /app/TestResults || true

# Final Image com live reload habilitado
FROM build AS final
WORKDIR /app
COPY --from=build /app/build .
EXPOSE 80

# Comando para rodar com live reload
CMD ["dotnet", "watch", "run", "--project", "VarzeaLeague.Application.csproj", "--urls", "http://+:443;http://*:80"]
