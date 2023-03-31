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
using System.Text;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace PortalMicroservice.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReplyLikeController : ControllerBase
    {
        private IConfiguration Configuration;
        Uri baseAddress;
        HttpClient client;
        public ReplyLikeController(IConfiguration configuration)
        {
            Configuration = configuration;
            baseAddress = new Uri(Configuration["Links:TweetMicroservice"]);
            client = new HttpClient();
            client.BaseAddress = baseAddress;
        }
        

        // GET api/<ReplyLikeController>/5
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            if (!IsAuthenticated())
                return Unauthorized("You are not authorized");
            try
            {
                
                string token = HttpContext.Request.Cookies["token"];
                var tokenHandler = new JwtSecurityTokenHandler();
                var securityToken = (JwtSecurityToken)tokenHandler.ReadToken(token);
                var claimEmail = securityToken.Claims.FirstOrDefault(c => c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress").Value;
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                HttpResponseMessage response = client.GetAsync(client.BaseAddress + "/Like/" + id+"/"+claimEmail).Result;
                if (response.IsSuccessStatusCode)
                {
                    string data = response.Content.ReadAsStringAsync().Result;
                    
                    return Ok(data);
                }
                return BadRequest("Tweet does not exist");
            }
            catch (Exception)
            {
                return Unauthorized("You are unauthorized");
            }
        }

       
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] string tweetPost)
        {
            if (!IsAuthenticated())
                return Unauthorized("You are not authorized");

            try
            {
                if (string.IsNullOrEmpty(tweetPost))
                {
                    return Ok("Tweet can not be empty");
                }
                Tweet tweet = new Tweet();
                tweet.Post = tweetPost;
                string token = HttpContext.Request.Cookies["token"];
                var tokenHandler = new JwtSecurityTokenHandler();
                var securityToken = (JwtSecurityToken)tokenHandler.ReadToken(token);
                var claimEmail = securityToken.Claims.FirstOrDefault(c => c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress").Value;
                tweet.PostedBy = claimEmail;
                string data = JsonConvert.SerializeObject(tweet);
                StringContent content = new StringContent(data, Encoding.UTF8, "application/json");
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                HttpResponseMessage response = client.PutAsync(client.BaseAddress + "/Like/"+id, content).Result;
                if (response.IsSuccessStatusCode)
                {
                    string message = response.Content.ReadAsStringAsync().Result;

                    return Ok(message);
                }

                return BadRequest("The tweet does not exist that you want to reply");
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
