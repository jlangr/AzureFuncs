namespace AzureFuncs.Models;

public record UpdateOrderRequest
{
    public string? Name { get; set; }
    public int? Quantity { get; set; }
    public decimal? Price { get; set; }
};