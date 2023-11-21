using System.Net;
using AzureFuncs.Models;
using AzureFuncs.Persistence;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace AzureFuncs.Functions;

public class UpdateOrderFunction
{
    private readonly ILogger logger;
    private readonly IDatabase database;

    public UpdateOrderFunction(ILoggerFactory loggerFactory, IDatabase database)
    {
        this.database = database;
        logger = loggerFactory.CreateLogger<UpdateOrderFunction>();
    }

    [Function("UpdateOrder")]
    public async Task<HttpResponseData> Run(
        [HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = "orders/{id}")] HttpRequestData req,
        Guid id)
    {
        logger.LogInformation("updating order");
        
        Console.WriteLine("updating order " + id);
        
        var orders = database.GetOrders();
        var order = orders.FirstOrDefault(o => o.Id == id);
        if (order == null) return req.CreateResponse(HttpStatusCode.NotFound);

        var requestBody = await new StreamReader(req.Body).ReadToEndAsync();
        var updated = JsonConvert.DeserializeObject<UpdateOrderRequest>(requestBody);
        if (updated == null) return req.CreateResponse(HttpStatusCode.BadRequest);

        order.Name = updated.Name ?? order.Name;
        order.Quantity = updated.Quantity ?? order.Quantity;
        order.Price = updated.Price ?? order.Price;
        order.Total = order.Quantity * order.Price;
        
        database.Update(order);

        var response = req.CreateResponse(HttpStatusCode.OK);
        await response.WriteAsJsonAsync(order);
        return response;
    }
}