# Etapa 1: Imagen base para ejecución
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080
ENV ASPNETCORE_URLS=http://+:8080

# Etapa 2: Imagen para compilación
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copiar el archivo del proyecto y restaurar dependencias
COPY ["Relatosxxx.csproj", "./"]
RUN dotnet restore "Relatosxxx.csproj"

# Copiar todo el código fuente
COPY . .

# Compilar el proyecto
RUN dotnet build "Relatosxxx.csproj" -c Release -o /app/build

# Etapa 3: Publicar la aplicación
FROM build AS publish
RUN dotnet publish "Relatosxxx.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Etapa 4: Imagen final
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

# Punto de entrada
ENTRYPOINT ["dotnet", "Relatosxxx.dll"]
