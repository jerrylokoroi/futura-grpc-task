# Build stage
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY common/Communication/Communication.Grpc/Communication.Grpc.csproj common/Communication/Communication.Grpc/
COPY api/OrderApi/OrderApi.csproj api/OrderApi/
RUN dotnet restore api/OrderApi/OrderApi.csproj

COPY common/ common/
COPY api/OrderApi/ api/OrderApi/
RUN dotnet publish api/OrderApi/OrderApi.csproj -c Release -o /app/publish

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "OrderApi.dll"]