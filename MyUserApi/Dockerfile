# STAGE 1: build (SDK)
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /app

# Kopiujemy plik csproj i przywracamy zależności
COPY MyUserApi.csproj .
COPY appsettings.json .
COPY Data ./Data
COPY Models ./Models
COPY Controllers ./Controllers

RUN dotnet restore

# Kopiujemy resztę plików
COPY . .

# Budujemy projekt (release)
RUN dotnet publish -c Release -o out

# STAGE 2: runtime (ASP.NET)
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app

# Kopiujemy efekt z poprzedniego etapu
COPY --from=build /app/out ./

ENTRYPOINT ["dotnet", "MyUserApi.dll"]
