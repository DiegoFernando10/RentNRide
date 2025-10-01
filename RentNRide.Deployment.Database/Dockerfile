# Build
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src
COPY . .
RUN dotnet publish RentNRide.Deployment.Database/RentNRide.Deployment.Database.csproj -c Release -o /app/publish

# Runtime
FROM mcr.microsoft.com/dotnet/runtime:9.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "RentNRide.Deployment.Database.dll"]
