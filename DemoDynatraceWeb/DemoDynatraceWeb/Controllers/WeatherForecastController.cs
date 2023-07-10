using System.Security.Cryptography;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using Microsoft.Net.Http.Headers;

namespace DemoDynatraceWeb.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

        private readonly ILogger<WeatherForecastController> _logger;

        #region MyRegion

         readonly string Json = @"[{""Name"":""Zaid"",""DemoField2"":""Khan""},{""DemoField3"":""Cloud Expert"",""DemoField4"":""Indian""}]";

        // Update customerId to your Log Analytics workspace ID
         readonly string CustomerId = "c8d966dc-16cd-42ce-b003-b199a5472dc4";

        // For sharedKey, use either the primary or the secondary Connected Sources client authentication key   
         readonly string SharedKey = "dHx1O2AuTempIXXCX5VGDkRWZavFSM/Ge3h7kuWn5L2t4YT3fO8qp6q23sqqZzQuahgabhhzxwcyOO9Y35AXPw==";

        // LogName is name of the event type that is being submitted to Azure Monitor
         readonly string LogName = "DyatraceLog";

        // You can use an optional field to specify the timestamp from the data. If the time field is not specified, Azure Monitor assumes the time is the message ingestion time
         readonly string TimeStampField = DateTime.UtcNow.ToString("r");
        // DateTime dt = new DateTime();
        
        
        // Build the API signature
        public  string BuildSignature(string message, string secret)
        {
            var encoding = new System.Text.ASCIIEncoding();
            byte[] keyByte = Convert.FromBase64String(secret);
            byte[] messageBytes = encoding.GetBytes(message);
            using (var hmacsha256 = new HMACSHA256(keyByte))
            {
                byte[] hash = hmacsha256.ComputeHash(messageBytes);
                return Convert.ToBase64String(hash);
            }
        }

        // Send a request to the POST API endpoint
        public  void PostData(string signature, string date, string json)
        {
            try
            {
                string url = "https://" + CustomerId + ".ods.opinsights.azure.com/api/logs?api-version=2016-04-01";

                System.Net.Http.HttpClient client = new System.Net.Http.HttpClient();
                client.DefaultRequestHeaders.Add("Accept", "application/json");
                client.DefaultRequestHeaders.Add("Log-Type", LogName);
                client.DefaultRequestHeaders.Add("Authorization", signature);
                client.DefaultRequestHeaders.Add("x-ms-date", date);
                client.DefaultRequestHeaders.Add("time-generated-field", TimeStampField);

                // If charset=utf-8 is part of the content-type header, the API call may return forbidden.
                System.Net.Http.HttpContent httpContent = new StringContent(json, Encoding.UTF8);
                httpContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
                Task<System.Net.Http.HttpResponseMessage> response = client.PostAsync(new Uri(url), httpContent);

                System.Net.Http.HttpContent responseContent = response.Result.Content;
                string result = responseContent.ReadAsStringAsync().Result;
                Console.WriteLine("Return Result: " + result);
            }
            catch (Exception ex)
            {
                Console.WriteLine("API Post Exception: " + ex.Message);
            }
        }

      
    

    #endregion

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }

        [HttpGet(Name = "GetWeatherForecast")]
        public IEnumerable<WeatherForecast> Get()
        {
            bool test = true;
            // Create a hash for the API signature
            string datestring = DateTime.UtcNow.ToString("r");
            var jsonBytes = Encoding.UTF8.GetBytes(Json);
            string stringToHash = "POST\n" + jsonBytes.Length + "\napplication/json\n" + "x-ms-date:" + datestring + "\n/api/logs";
            string hashedString = BuildSignature(stringToHash, SharedKey);
            string signature = "SharedKey " + CustomerId + ":" + hashedString;
            //while (test)
            //{
            PostData(signature, datestring, Json);
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
        }
    }
}