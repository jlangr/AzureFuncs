using System.Net;
using AzureFuncs.Persistence;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;

namespace AzureFuncs.Functions;

public class GetOrdersFunction
{
    private readonly ILogger logger;
    private readonly IDatabase database;

    public GetOrdersFunction(ILoggerFactory loggerFactory, IDatabase database)
    {
        logger = loggerFactory.CreateLogger<GetOrdersFunction>();
        this.database = database;
    }

    [Function("GetOrders")]
    public async Task<HttpResponseData> Run(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "orders")] HttpRequestData req)
    {
        logger.LogInformation("getting list of orders");

        var response = req.CreateResponse(HttpStatusCode.OK);
        await response.WriteAsJsonAsync(database.GetOrders());
        return response;
    }
}