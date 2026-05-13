using AutoMapper;
using Communication.Grpc.Protos.OrderService;
using OrderApi.Mappings;
using OrderApi.Services;
using OrderApi.ViewModels;

var builder = WebApplication.CreateBuilder(args);

AppContext.SetSwitch("System.Net.Http.SocketsHttpHandler.Http2UnencryptedSupport", true);

builder.Services.AddScoped<OrderServiceOut>();
builder.Services.AddAutoMapper(cfg => cfg.AddProfile<OrderMappingProfile>());
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
        policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseCors("AllowAll");
app.UseSwagger();
app.UseSwaggerUI();

app.MapGet("/orders", async (OrderServiceOut orderService) =>
    await orderService.GetOrdersAsync());

app.MapPost("/orders", async (OrderServiceOut orderService, CreateOrderViewModel dto) =>
{
    var request = new CreateOrderRequest
    {
        CustomerName = dto.CustomerName,
        Product = dto.Product,
        Quantity = dto.Quantity
    };
    var order = await orderService.CreateOrderAsync(request);
    return Results.Created($"/orders/{order.Id}", order);
});

app.Run();