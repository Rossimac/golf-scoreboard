#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Golf.Scoreboard.Api/Golf.Scoreboard.Api.csproj", "Golf.Scoreboard.Api/"]
RUN dotnet restore "Golf.Scoreboard.Api/Golf.Scoreboard.Api.csproj"
COPY . .
WORKDIR "/src/Golf.Scoreboard.Api"
RUN dotnet build "Golf.Scoreboard.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Golf.Scoreboard.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Golf.Scoreboard.Api.dll"]
