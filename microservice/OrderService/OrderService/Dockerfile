# Build stage
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY common/Communication/Communication.Grpc/Communication.Grpc.csproj common/Communication/Communication.Grpc/
COPY microservice/OrderService/OrderService/OrderService.csproj microservice/OrderService/OrderService/
RUN dotnet restore microservice/OrderService/OrderService/OrderService.csproj

COPY common/ common/
COPY microservice/OrderService/OrderService/ microservice/OrderService/OrderService/
RUN dotnet publish microservice/OrderService/OrderService/OrderService.csproj -c Release -o /app/publish

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "OrderService.dll"]