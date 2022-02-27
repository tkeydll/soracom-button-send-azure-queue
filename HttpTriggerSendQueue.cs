using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace tkey.soracom.button
{
    public static class HttpTriggerSendQueue
    {
        [FunctionName("HttpTriggerSendQueue")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)] HttpRequest req, 
            [Queue("buttonevent"),StorageAccount("AzureWebJobsStorage")] ICollector<string> queue, 
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();

            try
            {
                queue.Add(requestBody);
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult(ex.ToString());
            }

            return new OkObjectResult("Queue successed.");
        }
    }
}
