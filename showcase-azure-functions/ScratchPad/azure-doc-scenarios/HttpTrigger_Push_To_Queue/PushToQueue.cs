using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using System.Net;


namespace HttpTrigger_Push_To_Queue;

public class PushToQueue
{
    private readonly ILogger<PushToQueue> _logger;

    public PushToQueue(ILogger<PushToQueue> logger)
    {
        _logger = logger;
    }

    [Function("PushToQueue")]
    public MyQueueOutputType Run(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post")] HttpRequestData req)
    {
        _logger.LogInformation("Processing a request to write to local storage.");

        // 1. Create the web page response sent back to your browser
        var response = req.CreateResponse(HttpStatusCode.OK);
        response.WriteStringAsync("Successfully wrote a message to your local Azurite Queue!");

        // 2. Return the package containing BOTH the web page and the queue message
        return new MyQueueOutputType
        {
            HttpResponse = response,
            QueueMessage = $"Message generated from .NET 10 at: {DateTime.Now}"
        };
    }

    public class MyQueueOutputType
    {
        // Outbound component 1: The HTTP web response
        public HttpResponseData HttpResponse { get; set; }

        // Outbound component 2: The message sent to your local storage queue
        [QueueOutput("test-items-queue", Connection = "AzureWebJobsStorage")]
        public string QueueMessage { get; set; }
    }

    
}