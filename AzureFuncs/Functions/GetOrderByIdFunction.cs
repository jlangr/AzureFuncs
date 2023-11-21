using System.Net;
using AzureFuncs.Persistence;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;

namespace AzureFuncs.Functions;

public class GetOrderByIdFunction
{
    private readonly ILogger logger;
    private readonly IDatabase database;

    public GetOrderByIdFunction(ILoggerFactory loggerFactory, IDatabase database)
    {
        logger = loggerFactory.CreateLogger<GetOrderByIdFunction>();
        this.database = database;
    }

    [Function("GetOrderById")]
    public async Task<HttpResponseData> Run(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "orders/{id}")] HttpRequestData req,
        Guid id)
    {
        logger.LogInformation("retrieving an order");

        var orders = database.GetOrders();
        var order = orders.FirstOrDefault(o => o.Id == id);
        if (order == null) return req.CreateResponse(HttpStatusCode.NotFound);
        
        var response = req.CreateResponse(HttpStatusCode.OK);
        await response.WriteAsJsonAsync(order);
        return response;
    }
}