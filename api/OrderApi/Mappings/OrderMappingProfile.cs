using AutoMapper;
using Communication.Grpc.Protos.OrderService;
using Google.Protobuf.WellKnownTypes;
using OrderApi.ViewModels;

namespace OrderApi.Mappings;

public class OrderMappingProfile : Profile
{
    public OrderMappingProfile()
    {
        CreateMap<Timestamp, DateTime>().ConvertUsing(ts => ts.ToDateTime());
        CreateMap<OrderItem, OrderViewModel>();
    }
}