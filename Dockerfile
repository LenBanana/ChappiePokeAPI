#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /app
EXPOSE 80
EXPOSE 443

WORKDIR /src
COPY ["ChappiePokeAPI/ChappiePokeAPI.csproj", "ChappiePokeAPI/"]
RUN dotnet restore "ChappiePokeAPI/ChappiePokeAPI.csproj"
COPY . .
WORKDIR "/src/ChappiePokeAPI"
#RUN dotnet build "ChappiePokeAPI.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ChappiePokeAPI.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ChappiePokeAPI.dll"]