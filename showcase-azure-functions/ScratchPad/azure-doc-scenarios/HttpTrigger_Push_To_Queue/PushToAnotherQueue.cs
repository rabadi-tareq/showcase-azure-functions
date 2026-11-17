using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;

namespace HttpTrigger_Push_To_Queue
{
    public class PushToAnotherQueue
    {

        private readonly ILogger<PushToAnotherQueue> _logger;

        public PushToAnotherQueue(ILogger<PushToAnotherQueue> logger)
        {
            _logger = logger;
        }

        [Function("PushToAnotherQueue")]
        public async Task<CompoundReturnType> Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post")] HttpRequestData requestData)
        {
            _logger.LogInformation($"From Logger -> Running {nameof(PushToAnotherQueue)}");

            var response = requestData.CreateResponse();
            await response.WriteStringAsync("Successful execution");

            return new CompoundReturnType()
            {
                HttpResponseData = response,
                MainMessage = "Main message contenat"
            };
        }
        
    }

    public class CompoundReturnType
    {
        public HttpResponseData HttpResponseData { get; set; }

        [QueueOutput("Second-queue", Connection = "AzureWebJobsStorage")]
        public string MainMessage { get; set; }
    }
}
