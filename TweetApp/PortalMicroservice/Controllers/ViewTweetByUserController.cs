using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using PortalMicroservice.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace PortalMicroservice.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ViewTweetByUserController : ControllerBase
    {
        private IConfiguration Configuration;
        Uri baseAddress;
        HttpClient client;
        public ViewTweetByUserController(IConfiguration configuration)
        {
            Configuration = configuration;
            baseAddress = new Uri(Configuration["Links:TweetMicroservice"]);
            client = new HttpClient();
            client.BaseAddress = baseAddress;
        }
        
        // GET api/<ViewTweetByUserController>/5
        [HttpGet("{name}")]
        public IActionResult Get(string name)
        {
            if (!IsAuthenticated())
                return Unauthorized("You are not authorized");
            try
            {
                List<Tweet> list_tweet = new List<Tweet>();
                string token = HttpContext.Request.Cookies["token"];
               
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                HttpResponseMessage response = client.GetAsync(client.BaseAddress + "/Find/" + name).Result;
                if (response.IsSuccessStatusCode)
                {
                    string data = response.Content.ReadAsStringAsync().Result;
                    list_tweet = JsonConvert.DeserializeObject<List<Tweet>>(data);
                    return Ok(list_tweet);
                }
                return NotFound("No tweets have been posted such user");
            }
            catch (Exception)
            {
                return Unauthorized("You are unauthorized");
            }
        }

       
        private bool IsAuthenticated()
        {
            if (string.IsNullOrEmpty(HttpContext.Request.Cookies["token"]))
            {

                return false;
            }
            return true;
        }
    }
}
