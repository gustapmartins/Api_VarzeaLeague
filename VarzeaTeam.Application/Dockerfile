# Base Image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80

ENV ASPNETCORE_URLS=http://+:80
ENV ASPNETCORE_ENVIRONMENT=Development

# Build Image
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy project files and restore dependencies
COPY ["VarzeaTeam.Application/VarzeaLeague.Application.csproj", "VarzeaTeam.Application/"]
COPY ["VarzeaTeam.Infra.CrossCutting/VarzeaLeague.Infra.CrossCutting.csproj", "VarzeaTeam.Infra.CrossCutting/"]
COPY ["VarzeaTeam.Domain/VarzeaLeague.Domain.csproj", "VarzeaTeam.Domain/"]
COPY ["VarzeaTeam.Infra.Data/VarzeaLeague.Infra.Data.csproj", "VarzeaTeam.Infra.Data/"]
COPY ["VarzeaTeam.Test/VarzeaLeague.Test.csproj", "VarzeaTeam.Test/"]

RUN dotnet restore "VarzeaTeam.Application/VarzeaLeague.Application.csproj"

# Copy all files and build the project
COPY . .
WORKDIR "/src/VarzeaTeam.Application"
RUN dotnet build "VarzeaLeague.Application.csproj" -c Release -o /app/build

# Test Image
FROM build AS test
WORKDIR /src
RUN dotnet test "VarzeaTeam.Test/VarzeaLeague.Test.csproj" --configuration Release --no-build --verbosity normal --collect:"XPlat Code Coverage" --results-directory /app/TestResults || true

# Final Image with live reload enabled
FROM base AS final
WORKDIR /app
COPY --from=build /app/build .
EXPOSE 80

# Command to run with live reload
CMD ["dotnet", "watch", "run", "--project", "VarzeaLeague.Application.csproj", "--urls", "http://+:443;http://*:80"]
