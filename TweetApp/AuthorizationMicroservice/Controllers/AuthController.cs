using AuthorizationMicroservice.Authenticate;
using AuthorizationMicroservice.Model;
using AuthorizationMicroservice.Provider;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace AuthorizationMicroservice.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private IConfiguration Configuration;
        private readonly AuthorizationProvider _provider;
        public AuthController(IConfiguration configuration, AuthorizationProvider provider)
        {
            Configuration = configuration;
            _provider = provider;
        }
        // GET: api/<AuthController>
        //[HttpGet("{email}/{password}")]
        //public IActionResult Get(string email,string password)
        //{
        //    try
        //    {
        //        HttpResponseMessage response = client.GetAsync(client.BaseAddress + email + "/" + password).Result;
        //        if (response.IsSuccessStatusCode)
        //        {
        //            string message;
        //            string data = response.Content.ReadAsStringAsync().Result;
        //            message = data;
                  
                    
        //            string token = _auth.Authenticate(email, password);
        //            return Ok(token);
                    
                    
        //        }
        //        else
        //        {
        //            return Ok("User is not authenticated");
        //        }
        //    }
        //    catch(Exception ex)
        //    {
        //        throw new Exception("Communication failed "+ex.Message);
        //    }
           
        //}

        // GET api/<AuthController>/5
        //[HttpGet("{id}")]
        //public string Get(int id)
        //{
        //    return "value";
        //}

        // POST api/<AuthController>
        [HttpPost]
        public IActionResult Post([FromBody] LoginModel loginModel)
        {
            if (loginModel == null)
            {

                return StatusCode(500);
            }
            if (string.IsNullOrEmpty(loginModel.Email) || string.IsNullOrEmpty(loginModel.Password))
            {

                return BadRequest("Email/Password cannot be null");
            }
            try
            {
                if (_provider.ValidateUser(loginModel))
                {
                    TokenGenerator generator = new TokenGenerator(Configuration);
                    string token = generator.GenerateToken(loginModel.Email);

                    return Ok(token);
                }

                return Unauthorized("Invalid Credentials");
            }
            catch (Exception)
            {
                return StatusCode(500);
            }
        }

        // PUT api/<AuthController>/5
        //[HttpPut("{id}")]
        //public void Put(int id, [FromBody] string value)
        //{
        //}

        //// DELETE api/<AuthController>/5
        //[HttpDelete("{id}")]
        //public void Delete(int id)
        //{
        //}
    }
}
