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
    private readonly IMapper _mapper;

    public OrderServiceOut(IConfiguration configuration, IMapper mapper)
    {
        _configuration = configuration;
        _mapper = mapper;
    }

    private GrpcChannel CreateChannel()
    {
        var orderServiceUrl = _configuration["GrpcServices:OrderService"] ?? "http://localhost:5201";
        return GrpcChannel.ForAddress(orderServiceUrl, new GrpcChannelOptions
        {
            HttpHandler = new HttpClientHandler()
        });
    }

    public async Task<IReadOnlyList<OrderViewModel>> GetOrdersAsync()
    {
        using var channel = CreateChannel();
        var client = new OrderGrpcService.OrderServiceClient(channel);
        var response = await client.GetOrdersAsync(new Empty());
        return _mapper.Map<List<OrderViewModel>>(response.Orders);
    }

    public async Task<OrderViewModel> CreateOrderAsync(CreateOrderRequest request)
    {
        using var channel = CreateChannel();
        var client = new OrderGrpcService.OrderServiceClient(channel);
        var order = await client.CreateOrderAsync(request);
        return _mapper.Map<OrderViewModel>(order);
    }
}