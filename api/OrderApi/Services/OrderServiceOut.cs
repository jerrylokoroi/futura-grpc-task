using AutoMapper;
using Communication.Grpc.Protos.OrderService;
using Google.Protobuf.WellKnownTypes;
using Grpc.Net.Client;
using OrderApi.ViewModels;

namespace OrderApi.Services;

using OrderGrpcService = Communication.Grpc.Protos.OrderService.OrderService;

public class OrderServiceOut
{
    private readonly IConfiguration _configuration;

    public OrderServiceOut(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    private GrpcChannel CreateChannel()
    {
        var orderServiceUrl = _configuration["GrpcServices:OrderService"] ?? "http://localhost:5201";
        return GrpcChannel.ForAddress(orderServiceUrl, new GrpcChannelOptions
        {
            HttpHandler = new HttpClientHandler()
        });
    }

    private static OrderViewModel MapToViewModel(OrderItem order) =>
        new OrderViewModel(
            order.Id,
            order.CustomerName,
            order.Product,
            order.Quantity,
            order.CreatedAt.ToDateTime());

    public async Task<IReadOnlyList<OrderViewModel>> GetOrdersAsync()
    {
        using var channel = CreateChannel();
        var client = new OrderGrpcService.OrderServiceClient(channel);
        var response = await client.GetOrdersAsync(new Empty());
        return response.Orders.Select(MapToViewModel).ToList();
    }

    public async Task<OrderViewModel> CreateOrderAsync(CreateOrderRequest request)
    {
        using var channel = CreateChannel();
        var client = new OrderGrpcService.OrderServiceClient(channel);
        var order = await client.CreateOrderAsync(request);
        return MapToViewModel(order);
    }
}