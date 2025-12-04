# -------------------------------------------------------------------------
# ETAPA 1: BUILD (Compilación y Publicación)
# Esta etapa utiliza una imagen grande que contiene el SDK necesario para compilar C#.
# -------------------------------------------------------------------------
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# 1. Copiar Archivos de Proyectos y Solución:
# Copio solo los archivos .csproj y el .sln necesarios para la restauración de dependencias.
# Esto optimiza el caché de Docker si solo cambian los archivos de código fuente (.cs).
COPY ["Zoologico.API/Zoologico.API.csproj", "Zoologico.API/"]
COPY ["Zoologico.Modelos/Zoologico.Modelos.csproj", "Zoologico.Modelos/"]
COPY ["Zoologico.Modelos.sln", "."]

# 2. Restaurar Dependencias:
# Restauro las librerías NuGet necesarias para la aplicación.
RUN dotnet restore "Zoologico.API/Zoologico.API.csproj"

# 3. Copiar Código Fuente Completo:
# Copio el resto del código fuente del repositorio.
COPY . .

# 4. Publicar la Aplicación:
# Me muevo al directorio del proyecto API y genero la versión de producción (Release) en la carpeta /app/publish.
WORKDIR "/src/Zoologico.API"
RUN dotnet publish "Zoologico.API.csproj" -c Release -o /app/publish


# -------------------------------------------------------------------------
# ETAPA 2: FINAL (Ejecución en Producción)
# Esta etapa usa una imagen liviana que solo contiene el runtime necesario para ejecutar la DLL.
# -------------------------------------------------------------------------
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app

# 5. Copiar Archivos Publicados:
# Copio la salida de la compilación (/app/publish) desde la etapa 'build' a este contenedor final.
COPY --from=build /app/publish .

# 6. Punto de Entrada (Startup):
# Comando que Render usará para iniciar la aplicación (la DLL tiene el mismo nombre que el proyecto API).
ENTRYPOINT ["dotnet", "Zoologico.API.dll"]
