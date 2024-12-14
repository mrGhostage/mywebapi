# Используем официальный образ .NET для сборки
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Копируем проект и устанавливаем зависимости
COPY *.csproj ./
RUN dotnet restore

# Копируем оставшиеся файлы и собираем приложение
COPY . ./
RUN dotnet publish -c Release -o out

# Используем минимальный runtime-образ для запуска
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/out .
ENTRYPOINT ["dotnet", "MyWebAPI.dll"]