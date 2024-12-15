# Используем официальный образ .NET SDK для сборки приложения
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /app
# EXPOSE 8080

# Копируем файл проекта (*.csproj) в рабочую директорию контейнера
COPY MyWebAPI/*.csproj ./

# Выполняем восстановление зависимостей
RUN dotnet restore

# Копируем все остальные файлы проекта
COPY MyWebAPI/ ./

# Собираем проект в режиме Release
RUN dotnet publish -c Release -o out

# Используем минимальный runtime-образ для запуска
FROM mcr.microsoft.com/dotnet/aspnet:9.0
WORKDIR /app
COPY --from=build /app/out .

# ENV ASPNETCORE_URLS=http://+:8080
# Указываем точку входа для запуска приложения
ENTRYPOINT ["dotnet", "MyWebAPI.dll"]