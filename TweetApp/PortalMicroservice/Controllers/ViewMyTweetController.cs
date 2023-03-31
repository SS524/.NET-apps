using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using PortalMicroservice.Model;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace PortalMicroservice.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ViewMyTweetController : ControllerBase
    {
        private IConfiguration Configuration;
        Uri baseAddress;
        HttpClient client;
        public ViewMyTweetController(IConfiguration configuration)
        {
            Configuration = configuration;
            baseAddress = new Uri(Configuration["Links:TweetMicroservice"]);
            client = new HttpClient();
            client.BaseAddress = baseAddress;
        }
        // GET: api/<ViewMyTweetController>
        [HttpGet]
        public IActionResult Get()
        {
            if (!IsAuthenticated())
                return Unauthorized("You are not authorized");
            try
            {
                List<Tweet> list_tweet = new List<Tweet>();
                string token = HttpContext.Request.Cookies["token"];
                var tokenHandler = new JwtSecurityTokenHandler();
                var securityToken = (JwtSecurityToken)tokenHandler.ReadToken(token);
                var claimEmail = securityToken.Claims.FirstOrDefault(c => c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress").Value;
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                HttpResponseMessage response = client.GetAsync(client.BaseAddress + "/Tweet/" + claimEmail).Result;
                if (response.IsSuccessStatusCode)
                {
                    string data = response.Content.ReadAsStringAsync().Result;
                    list_tweet = JsonConvert.DeserializeObject<List<Tweet>>(data);
                    return Ok(list_tweet);
                }
                return NotFound("You have not posted any tweets");
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
