using System.Net;
using AzureFuncs.Models;
using AzureFuncs.Persistence;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace AzureFuncs.Functions;

public class CreateOrderFunction
{
    private readonly ILogger logger;
    private readonly IDatabase database;

    public CreateOrderFunction(ILoggerFactory loggerFactory, IDatabase database)
    {
        logger = loggerFactory.CreateLogger<CreateOrderFunction>();
        this.database = database;
    }

    [Function("CreateOrder")]
    public async Task<HttpResponseData> Run(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route="orders")] HttpRequestData req)
    {
        logger.LogInformation("creating a new order");

        var requestBody = await new StreamReader(req.Body).ReadToEndAsync();

        var request = JsonConvert.DeserializeObject<CreateOrderRequest>(requestBody);
        if (request == null) return req.CreateResponse(HttpStatusCode.BadRequest);

        var order = new Order
        {
            Id = Guid.NewGuid(),
            Name = request.Name,
            Price = request.Price,
            Quantity = request.Quantity,
            Total = request.Price * request.Quantity
        };
        
        database.Add(order);
        
        var response = req.CreateResponse(HttpStatusCode.OK);
        await response.WriteAsJsonAsync(order);
        return response;
    }
}