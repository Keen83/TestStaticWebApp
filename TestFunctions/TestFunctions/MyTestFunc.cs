using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace TestFunctions
{
    public static class MyTestFunc
    {
        [FunctionName("MyTestFunc")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)]
            MyModel getData,
            HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");
            
            string name = req.Query["name"];
            string age = req.Query["age"];

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var postData = JsonConvert.DeserializeObject<MyModel>(requestBody);
            var model = getData ?? postData;

            string responseMessage = model == null
                ? "This HTTP triggered function executed successfully. Pass a name in the query string or in the request body for a personalized response."
                : $"Hello, {model.Name}. This HTTP triggered function executed successfully. Age: {model.Age}";

            return new OkObjectResult(responseMessage);
        }
    }

    public class MyModel
    {
        public int Age { get; set; }
        public string Name { get; set; }
    }
}


