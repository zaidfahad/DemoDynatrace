using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

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
                var a = 22 / x;
                _logger.LogInformation("Run " + DateTime.Now.ToString("R"));
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
}
