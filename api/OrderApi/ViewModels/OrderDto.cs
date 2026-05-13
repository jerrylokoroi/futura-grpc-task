namespace OrderApi.ViewModels;

public record OrderViewModel(
    int Id,
    string CustomerName,
    string Product,
    int Quantity,
    DateTime CreatedAt);

public record CreateOrderViewModel(
    string CustomerName,
    string Product,
    int Quantity);