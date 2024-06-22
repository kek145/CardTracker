FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build-env

WORKDIR /src

COPY CardTracker.Infrastructure/*csproj CardTracker.Infrastructure/
COPY CardTracker.Domain/*csproj CardTracker.Domain/
COPY CardTracker.Application/*csproj CardTracker.Application/
COPY CardTracker.Api/*csproj CardTracker.Api/

RUN dotnet restore CardTracker.Infrastructure/
RUN dotnet restore CardTracker.Application/
RUN dotnet restore CardTracker.Domain/
RUN dotnet restore CardTracker.Api/

COPY CardTracker.Infrastructure/. CardTracker.Infrastructure/
COPY CardTracker.Application/. CardTracker.Application/
COPY CardTracker.Domain/. CardTracker.Domain/

FROM build-env AS build-app

WORKDIR /src/CardTracker.Api

COPY CardTracker.Api/. .

RUN dotnet build -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime

WORKDIR /app

COPY --from=build-app /app/publish .

EXPOSE 80

ENV ASPNETCORE_ENVIRONMENT=Production

ENTRYPOINT ["dotnet", "CardTracker.Api.dll"]