using System.Net;
using AzureFuncs.Persistence;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;

namespace AzureFuncs.Functions;

public class DeleteOrderFunction
{
    private readonly ILogger logger;
    private readonly IDatabase database;

    public DeleteOrderFunction(ILoggerFactory loggerFactory, IDatabase database)
    {
        logger = loggerFactory.CreateLogger<DeleteOrderFunction>();
        this.database = database;
    }

    [Function("DeleteOrder")]
    public HttpResponseData Run(
        [HttpTrigger(AuthorizationLevel.Anonymous, "delete", Route = "orders/{id}")] HttpRequestData req,
        Guid id)
    {
        logger.LogInformation("deleting order");

        var orders = database.GetOrders();
        var order = orders.FirstOrDefault(o => o.Id == id);
        if (order == null) return req.CreateResponse(HttpStatusCode.NotFound);
        
        database.Remove(order);

        return req.CreateResponse(HttpStatusCode.NoContent);
    }
}