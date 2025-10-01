# ===== Build Stage =====
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src
COPY . .

RUN dotnet publish -c Release -r linux-x64 --self-contained true -o /app/publish /p:PublishTrimmed=false

# ===== Runtime Stage =====
FROM mcr.microsoft.com/dotnet/runtime-deps:9.0
WORKDIR /app

COPY --from=build /app/publish .
ENTRYPOINT ["./RentNRide.Deployment.Database"]
