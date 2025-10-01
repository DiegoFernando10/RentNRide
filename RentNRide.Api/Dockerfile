# Build stage
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

COPY RentNRide.Api/RentNRide.Api.csproj RentNRide.Api/
COPY RentNRide.sln ./

RUN dotnet restore RentNRide.Api/RentNRide.Api.csproj
COPY . .

RUN dotnet publish RentNRide.Api/RentNRide.Api.csproj -c Release -o /app

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:9.0
WORKDIR /app
COPY --from=build /app .
ENTRYPOINT ["dotnet", "RentNRide.Api.dll"]
