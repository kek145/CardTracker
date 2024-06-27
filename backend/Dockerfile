# Установка SDK для сборки
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build-env

# Установка рабочей директории
WORKDIR /src

# Копирование файла решения и восстановление зависимостей
COPY backend.sln ./
COPY CardTracker.Infrastructure/*.csproj CardTracker.Infrastructure/
COPY CardTracker.Domain/*.csproj CardTracker.Domain/
COPY CardTracker.Application/*.csproj CardTracker.Application/
COPY CardTracker.Api/*.csproj CardTracker.Api/

RUN dotnet restore

# Копирование всех файлов проекта
COPY . ./

# Сборка и публикация проекта
WORKDIR /src/CardTracker.Api
RUN dotnet publish -c Release -o /app/publish

# Установка среды выполнения
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime

# Установка рабочей директории
WORKDIR /app

# Копирование сборки из стадии сборки
COPY --from=build-env /app/publish .

# Открытие порта
EXPOSE 80

# Настройка переменной окружения
ENV ASPNETCORE_ENVIRONMENT=Production

# Определение точки входа
ENTRYPOINT ["dotnet", "CardTracker.Api.dll"]
