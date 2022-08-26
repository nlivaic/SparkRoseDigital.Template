#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/runtime:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["src/SparkRoseDigital_Template.Migrations/SparkRoseDigital_Template.Migrations.csproj", "src/SparkRoseDigital_Template.Migrations/"]
RUN dotnet restore "src/SparkRoseDigital_Template.Migrations/SparkRoseDigital_Template.Migrations.csproj"
COPY . .
WORKDIR "/src/src/SparkRoseDigital_Template.Migrations"
RUN dotnet build "SparkRoseDigital_Template.Migrations.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "SparkRoseDigital_Template.Migrations.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SparkRoseDigital_Template.Migrations.dll", " ", " ", " "]