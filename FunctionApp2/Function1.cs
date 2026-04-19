using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Text.Json;

namespace FunctionApp2;

public class Function1
{
    private readonly ILogger<Function1> _logger;

    public Function1(ILogger<Function1> logger)
    {
        _logger = logger;
    }
    [Function("GetProductById")]
    public async Task<HttpResponseData> GetProductById([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "products/{id}")] HttpRequestData req)
    {
        _logger.LogInformation("GetProducts function triggered.");

        var products = new[]
        {
            new { Id = 1, Name = "Azure T-Shirt",   Price = 29.99 },
            new { Id = 2, Name = "Cloud Mug",        Price = 14.99 },
            new { Id = 3, Name = "DevOps Sticker",   Price = 4.99  }
        };

        var response = req.CreateResponse(HttpStatusCode.OK);
        response.Headers.Add("Content-Type", "application/json");
        await response.WriteStringAsync(JsonSerializer.Serialize(products));
        return response;
    }

    [Function("GetProducts")]
    public async Task<HttpResponseData> GetProducts([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "products")]HttpRequestData req)
    {
        _logger.LogInformation("GetProducts function triggered.");

        var products = new[]
        {
            new { Id = 1, Name = "Sanjay-Azure T-Shirt",   Price = 29.99 },
            new { Id = 2, Name = "Mishra-Cloud Mug",        Price = 14.99 },
            new { Id = 3, Name = "DevOps Sticker",   Price = 4.99  }
        };

        var response = req.CreateResponse(HttpStatusCode.OK);
        response.Headers.Add("Content-Type", "application/json");
        await response.WriteStringAsync(JsonSerializer.Serialize(products));
        return response;
    }

    [Function("CreateProduct")]
    public async Task<HttpResponseData> Create([HttpTrigger(AuthorizationLevel.Function, "post", Route = "products")]HttpRequestData req)
    {
        var body = await new StreamReader(req.Body).ReadToEndAsync();
        var product = JsonSerializer.Deserialize<dynamic>(body);

        _logger.LogInformation("New product created: {Body}", body);

        var response = req.CreateResponse(HttpStatusCode.Created);
        await response.WriteStringAsync("{\"message\": \"Product created.\"}");
        return response;
    }
}