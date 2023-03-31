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
    public class TweetController : ControllerBase
    {
        private IConfiguration Configuration;
        Uri baseAddress;
        HttpClient client;
        public TweetController(IConfiguration configuration)
        {
            Configuration = configuration;
            baseAddress = new Uri(Configuration["Links:TweetMicroservice"]);
            client = new HttpClient();
            client.BaseAddress = baseAddress;
        }
        // GET: api/<TweetController>
        [HttpGet]
        public IActionResult Get()
        {
            if (!IsAuthenticated())
                return Unauthorized("You are not authorized");
            try {
                List<Tweet> list_tweet = new List<Tweet>();
                string token = HttpContext.Request.Cookies["token"];
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                HttpResponseMessage response = client.GetAsync(client.BaseAddress + "/Tweet").Result;
                if (response.IsSuccessStatusCode)
                {
                    string data = response.Content.ReadAsStringAsync().Result;
                    list_tweet= JsonConvert.DeserializeObject<List<Tweet>>(data);
                    return Ok(list_tweet);
                }
                return NotFound("No tweets avaialble");
            }
            catch (Exception)
            {
                return Unauthorized("You are unauthorized");
            }


        }

        

        // POST api/<TweetController>
        [HttpPost]
        public IActionResult Post([FromBody] string tweetPost)
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
                HttpResponseMessage response = client.PostAsync(client.BaseAddress+"/Tweet", content).Result;
                if (response.IsSuccessStatusCode)
                {
                    string message = response.Content.ReadAsStringAsync().Result;

                    return Ok(message);
                }

                return Unauthorized("You are unauthorized");
            }
            catch (Exception)
            {
                return Unauthorized("You are unauthorized");
            }
        }

        // PUT api/<TweetController>/5
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] string tweetPost)
        {
            if (!IsAuthenticated())
                return Unauthorized("You are not authorized");
            try
            { List<Tweet> list_tweet = new List<Tweet>();
                string token = HttpContext.Request.Cookies["token"];
                var tokenHandler = new JwtSecurityTokenHandler();
                var securityToken = (JwtSecurityToken)tokenHandler.ReadToken(token);
                var claimEmail = securityToken.Claims.FirstOrDefault(c => c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress").Value;
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                HttpResponseMessage response = client.GetAsync(client.BaseAddress + "/Tweet/"+claimEmail).Result;
               
                if (response.IsSuccessStatusCode)
                {
                    string data = response.Content.ReadAsStringAsync().Result;
                    list_tweet = JsonConvert.DeserializeObject<List<Tweet>>(data);
                    var obj = list_tweet.Any(t => t.Id == id);
                    if (obj == false)
                    {
                        return Ok("You can not edit this tweet");
                    }

                }
                else
                {
                    return BadRequest("You have no tweets to edit");
                }
                string data2 = JsonConvert.SerializeObject(tweetPost);
                StringContent content = new StringContent(data2, Encoding.UTF8, "application/json");
                HttpResponseMessage response2 = client.PutAsync(client.BaseAddress + "/Tweet/"+id, content).Result;
                if (response2.IsSuccessStatusCode)
                {
                    string message = response2.Content.ReadAsStringAsync().Result;

                    return Ok(message);
                }

                return BadRequest("");


            }
            catch (Exception)
            {
                return BadRequest("");
            }
        }

        // DELETE api/<TweetController>/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
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
                    var obj = list_tweet.Any(t => t.Id == id);
                    if (obj == false)
                    {
                        return Ok("You can not delete this tweet");
                    }

                }
                else
                {
                    return BadRequest("You have no tweets to delete");
                }
                
                HttpResponseMessage response2 = client.DeleteAsync(client.BaseAddress + "/Tweet/" + id).Result;
                if (response2.IsSuccessStatusCode)
                {
                    string message = response2.Content.ReadAsStringAsync().Result;

                    return Ok(message);
                }

                return BadRequest("");


            }
            catch (Exception)
            {
                return Unauthorized("You are not authorized");
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
