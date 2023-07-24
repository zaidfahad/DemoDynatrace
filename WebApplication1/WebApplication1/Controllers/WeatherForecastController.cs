using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace WebApplication1.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool",
            "Mild", "Warm", "Balmy", "Hot", "Sweltering",
            "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public async Task<string> GetAsync()
        {

            return await FunctionPointerAttributesAsync(); ;
        }

        ///
        private async Task<string> FunctionPointerAttributesAsync()
        {
            var rootList = new List<Root>();
            var person = new Root
            {
                content = "Zaid Demo",
                logsource = "Zaid Demo",
                timestamp = DateTime.UtcNow,
                severity = "fatal",
                customattribute = "Home Page Issue",
                loglevel = "error",
                status = "error"
            };
            rootList.Clear();
            rootList.Add(person);
            var json = Newtonsoft.Json.JsonConvert.SerializeObject(rootList);
            var data = new System.Net.Http.StringContent(json, Encoding.UTF8, "application/json");

            var url = "https://dof15210.live.dynatrace.com/api/v2/logs/ingest";
            using var cl = new HttpClient();
            //
            string _ContentType = "application/json";
            cl.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(_ContentType));

            cl.DefaultRequestHeaders.Add("Authorization", "Api-Token dt0c01.CKN2RE4RFWKZZJH2B2ZSCIPR.MTNEGOVSDBBPZDNKORVSJD3KYUOVMQT67YGQ5EAGP4MTNMCJTERSQJHRGOIL6WEJ");
            //var userAgent = "d-fens HttpClient";
            //cl.DefaultRequestHeaders.Add("User-Agent", userAgent);
            //
            var response = await cl.PostAsync(url, data);

            string result = response.Content.ReadAsStringAsync().Result;
            return "Logs is sent to Dynatrace";
        }
    }
    public class Root
    {
        public string content { get; set; }

        [JsonProperty("log.source")]
        public string logsource { get; set; }
        public DateTime timestamp { get; set; }
        public string severity { get; set; }

        [JsonProperty("custom.attribute")]
        public string customattribute { get; set; }
        public string loglevel { get; set; }
        public string status { get; set; }
    }

}
