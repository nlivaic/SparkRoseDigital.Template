#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/runtime:9.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src
COPY ["src/SparkRoseDigital_Template.WorkerServices/SparkRoseDigital_Template.WorkerServices.csproj", "src/SparkRoseDigital_Template.WorkerServices/"]
COPY ["src/SparkRoseDigital_Template.Core/SparkRoseDigital_Template.Core.csproj", "src/SparkRoseDigital_Template.Core/"]
COPY ["src/SparkRoseDigital_Template.Common/SparkRoseDigital_Template.Common.csproj", "src/SparkRoseDigital_Template.Common/"]
RUN dotnet restore "src/SparkRoseDigital_Template.WorkerServices/SparkRoseDigital_Template.WorkerServices.csproj"
COPY . .
WORKDIR "/src/src/SparkRoseDigital_Template.WorkerServices"
RUN dotnet build "SparkRoseDigital_Template.WorkerServices.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "SparkRoseDigital_Template.WorkerServices.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
COPY ["src/SparkRoseDigital_Template.WorkerServices/entrypoint.sh", "src/entrypoint.sh"]
COPY ["src/SparkRoseDigital_Template.WorkerServices/wait-for-it.sh", "src/wait-for-it.sh"]
CMD ["/bin/bash", "src/entrypoint.sh"]
