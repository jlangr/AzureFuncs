using AzureFuncs.Models;

namespace AzureFuncs.Persistence;

public interface IDatabase
{
    void Add(Order order);
    IEnumerable<Order> GetOrders();
    void Remove(Order order);
    void Update(Order order);
}