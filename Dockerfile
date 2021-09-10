FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build
WORKDIR /app

COPY Lapka.Communication.Api/Lapka.Communication.Api.csproj Lapka.Communication.Api/Lapka.Communication.Api.csproj
COPY Lapka.Communication.Application/Lapka.Communication.Application.csproj Lapka.Communication.Application/Lapka.Communication.Application.csproj
COPY Lapka.Communication.Core/Lapka.Communication.Core.csproj Lapka.Communication.Core/Lapka.Communication.Core.csproj
COPY Lapka.Communication.Infrastructure/Lapka.Communication.Infrastructure.csproj Lapka.Communication.Infrastructure/Lapka.Communication.Infrastructure.csproj
RUN dotnet restore Lapka.Communication.Api

COPY . .
RUN dotnet publish Lapka.Communication.Api -c release -o out


FROM mcr.microsoft.com/dotnet/core/aspnet:3.1
WORKDIR /app
COPY --from=build /app/out .
ENV ASPNETCORE_URLS http://*:5004
ENV ASPNETCORE_ENVIRONMENT Docker

EXPOSE 5004

ENTRYPOINT dotnet Lapka.Communication.Api.dll