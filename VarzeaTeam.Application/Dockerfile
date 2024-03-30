#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

#Depending on the operating system of the host machines(s) that will build or run the containers, the image specified in the FROM statement may need to be changed.
#For more information, please see https://aka.ms/containercompat

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["VarzeaTeam.Application/VarzeaLeague.Application.csproj", "VarzeaTeam.Application/"]
COPY ["VarzeamTeam.Infra.CrossCutting/VarzeamLeague.Infra.CrossCutting.csproj", "VarzeamTeam.Infra.CrossCutting/"]
COPY ["VarzeaTeam.Domain/VarzeaLeague.Domain.csproj", "VarzeaTeam.Domain/"]
COPY ["VarzeaTeam.Infra.Data/VarzeaLeague.Infra.Data.csproj", "VarzeaTeam.Infra.Data/"]
RUN dotnet restore "VarzeaTeam.Application/VarzeaLeague.Application.csproj"
COPY . .
WORKDIR "/src/VarzeaTeam.Application"
RUN dotnet build "VarzeaLeague.Application.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "VarzeaLeague.Application.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "VarzeaLeague.Application.dll"]