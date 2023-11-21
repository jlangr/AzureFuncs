using AzureFuncs.Models;

namespace AzureFuncs.Persistence;

public class InMemoryDatabase: IDatabase
{
    private static List<Order> orders = new List<Order>();
    
    public void Add(Order order)
    {
        orders.Add(order);
    }

    public IEnumerable<Order> GetOrders()
    {
        return orders;
    }

    public void Remove(Order order)
    {
        orders.Remove(order);
    }

    public void Update(Order order)
    {
        orders.RemoveAll(x => x.Id == order.Id);
        orders.Add(order);
    }
}