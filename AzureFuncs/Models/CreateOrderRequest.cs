namespace AzureFuncs.Models;

public record CreateOrderRequest(string Name, int Quantity, decimal Price);