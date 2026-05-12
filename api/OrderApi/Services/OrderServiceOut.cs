using Communication.Grpc.Protos.OrderService;
using Google.Protobuf.WellKnownTypes;
using Grpc.Net.Client;

namespace OrderApi.Services;

using OrderGrpcService = Communication.Grpc.Protos.OrderService.OrderService;

public class OrderServiceOut
{
    private readonly IConfiguration _configuration;

    public OrderServiceOut(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task<IReadOnlyList<OrderDto>> GetOrdersAsync()
    {
        var orderServiceUrl = _configuration["GrpcServices:OrderService"] ?? "http://localhost:5201";

        using var channel = GrpcChannel.ForAddress(orderServiceUrl, new GrpcChannelOptions
        {
            HttpHandler = new HttpClientHandler()
        });        var client = new OrderGrpcService.OrderServiceClient(channel);
        var response = await client.GetOrdersAsync(new Empty());

        return response.Orders
            .Select(order => new OrderDto(
                order.Id,
                order.CustomerName,
                order.Product,
                order.Quantity,
                order.CreatedAt.ToDateTime()))
            .ToList();
    }
}

public record OrderDto(
    int Id,
    string CustomerName,
    string Product,
    int Quantity,
    DateTime CreatedAt);