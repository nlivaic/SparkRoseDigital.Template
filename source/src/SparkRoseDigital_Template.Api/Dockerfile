#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
# Uncomment if you have other certs this services needs to trust.
#COPY nginx/id-local.crt /usr/local/share/ca-certificates/id-local.crt
#RUN update-ca-certificates
EXPOSE 5000

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["src/SparkRoseDigital_Template.Api/SparkRoseDigital_Template.Api.csproj", "src/SparkRoseDigital_Template.Api/"]
COPY ["src/SparkRoseDigital_Template.Data/SparkRoseDigital_Template.Data.csproj", "src/SparkRoseDigital_Template.Data/"]
COPY ["src/SparkRoseDigital_Template.Common/SparkRoseDigital_Template.Common.csproj", "src/SparkRoseDigital_Template.Common/"]
COPY ["src/SparkRoseDigital_Template.Core/SparkRoseDigital_Template.Core.csproj", "src/SparkRoseDigital_Template.Core/"]
RUN dotnet restore "src/SparkRoseDigital_Template.Api/SparkRoseDigital_Template.Api.csproj"
COPY . .
WORKDIR "/src/src/SparkRoseDigital_Template.Api"
RUN dotnet build "SparkRoseDigital_Template.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "SparkRoseDigital_Template.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SparkRoseDigital_Template.Api.dll"]