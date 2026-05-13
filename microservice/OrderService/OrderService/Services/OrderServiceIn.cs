using Communication.Grpc.Protos.OrderService;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Microsoft.EntityFrameworkCore;
using OrderService.Data;
using OrderService.Models;

namespace OrderService.Services;

using OrderGrpcService = Communication.Grpc.Protos.OrderService.OrderService;

public class OrderServiceIn : OrderGrpcService.OrderServiceBase
{
    private readonly AppDbContext _dbContext;

    public OrderServiceIn(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public override async Task<GetOrdersResponse> GetOrders(Empty request, ServerCallContext context)
    {
        var orders = await _dbContext.Orders
            .AsNoTracking()
            .OrderBy(order => order.Id)
            .ToListAsync(context.CancellationToken);

        var response = new GetOrdersResponse();
        response.Orders.AddRange(orders.Select(order => new OrderItem
        {
            Id = order.Id,
            CustomerName = order.CustomerName,
            Product = order.Product,
            Quantity = order.Quantity,
            CreatedAt = ToTimestamp(order.CreatedAt)
        }));

        return response;
    }

    public override async Task<OrderItem> CreateOrder(CreateOrderRequest request, ServerCallContext context)
    {
        var order = new Order
        {
            CustomerName = request.CustomerName,
            Product = request.Product,
            Quantity = request.Quantity,
            CreatedAt = DateTime.UtcNow
        };

        _dbContext.Orders.Add(order);
        await _dbContext.SaveChangesAsync(context.CancellationToken);

        return new OrderItem
        {
            Id = order.Id,
            CustomerName = order.CustomerName,
            Product = order.Product,
            Quantity = order.Quantity,
            CreatedAt = ToTimestamp(order.CreatedAt)
        };
    }

    private static Timestamp ToTimestamp(DateTime dateTime)
    {
        return Timestamp.FromDateTime(dateTime.Kind == DateTimeKind.Utc
            ? dateTime
            : DateTime.SpecifyKind(dateTime, DateTimeKind.Utc));
    }
}