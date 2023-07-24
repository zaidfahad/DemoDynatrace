using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net.Http;
using System.Reflection.Metadata;
using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Dynatrace15july2023.Controllers
{
    [Route("api/home")]
    [ApiController]
    public class HomeController1 : ControllerBase
    {
        private readonly ILogger<HomeController1> _logger;

        public HomeController1(ILogger<HomeController1> logger)
        {
            _logger = logger;
        }
        // GET: api/<HomeController1>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            int x = 0;
            try
            {
                var time = DateTime.UtcNow;
                string postData = "[\r\n  {\r\n    \"content\": \"Exception: Custom error log sent via Generic Log Ingest\",\r\n    \"log.source\": \"/var/log/syslog\",\r\n    \"timestamp\": \"'+time+'\",\r\n    \"severity\": \"error\",\r\n    \"custom.attribute\": \"attribute value\"\r\n  }]";
                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri("https://jsonplaceholder.typicode.com/");

                var json = System.Text.Json.JsonSerializer.Serialize(postData);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = client.PostAsync("posts", content).Result;
                if (response.IsSuccessStatusCode)
                {
                    var responseContent = response.Content.ReadAsStringAsync().Result;

                    var options = new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    };
                    var PostResponse = new PostData
                    {
                        logsource = "",
                        timestamp=DateTime.UtcNow,
                        severity="critical",
                        customattribute=""
                    };
                    var postResponse = System.Text.Json.JsonSerializer.Deserialize<PostResponse>(responseContent, options);
                    Console.WriteLine("Post successful! ID: " + postResponse.Id);

                }
                else
                {
                    Console.WriteLine("Error: " + response.StatusCode);
                }
            }
            catch (Exception ex)
            {
                _logger.LogWarning("An example of a Warning trace..");
                _logger.LogError("An example of an Error level message");
            }
            return new string[] { "Completed", "Done" };
        }

        // GET api/<HomeController1>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            string test = "";
            try
            {
                var a = 22 / id;
                test = "Done";
                _logger.LogInformation("Run "+DateTime.Now.ToString("R"));
            }
            catch (Exception ex)
            {
                test = ex.Message;
                _logger.LogWarning("An example of a Warning trace..");
                _logger.LogError("An example of an Error level message");
            }
            return test;
        }

        // POST api/<HomeController1>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<HomeController1>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<HomeController1>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
    public class PostData
    {
        public string content { get; set; }

        [JsonProperty("log.source")]
        public string logsource { get; set; }
        public object timestamp { get; set; }
        public string severity { get; set; }

        [JsonProperty("custom.attribute")]
        public string customattribute { get; set; }
    }
}
