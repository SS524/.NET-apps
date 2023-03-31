using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using PortalMicroservice.Model;
using PortalMicroservice.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace PortalMicroservice.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginLogoutController : ControllerBase
    {
        private IConfiguration Configuration;
        private readonly IValidations _validations;
        Uri baseAddress;
        HttpClient client;
        public LoginLogoutController(IConfiguration configuration,IValidations validations)
        {
            _validations = validations;
            Configuration = configuration;
            baseAddress = new Uri(Configuration["Links:AuthorizationMicroservice"]);
            client = new HttpClient();
            client.BaseAddress = baseAddress;
        }
        // GET: api/<LoginLogoutController>
        [HttpGet]
        public IActionResult Logout()
        {
            string token= HttpContext.Request.Cookies["token"];
            if (token == null)
            {
                return Ok("You are already logged out");
            }
            HttpContext.Response.Cookies.Delete("token");

            return Ok("You are logged out");
        }

       

        // POST api/<LoginLogoutController>
        [HttpPost]
        public IActionResult Authenticate([FromBody] LoginModel loginModel)
        {
            try
            {
                string tokenCheck = HttpContext.Request.Cookies["token"];
                if (tokenCheck != null)
                {
                    return Ok("You have an active session. Please logout to login again");
                }
                if (!_validations.EmailValidation(loginModel.Email))
                {
                    return Ok("Please provide valid email address");
                }
                if (!_validations.PasswordValidation(loginModel.Password))
                {
                    return Ok("Please provide valid password");
                }
                if(!_validations.EmailValidation(loginModel.Email) && !_validations.PasswordValidation(loginModel.Password))
                {
                    return Ok("Please provide valid email and password");
                }
                string data = JsonConvert.SerializeObject(loginModel);
                StringContent content = new StringContent(data, Encoding.UTF8, "application/json");
                HttpResponseMessage response = client.PostAsync(client.BaseAddress, content).Result;
                if (response.IsSuccessStatusCode)
                {
                    string token = response.Content.ReadAsStringAsync().Result;
                    HttpContext.Response.Cookies.Append("token", token);
                    return Ok("You are logged in");
                }

                return BadRequest("Invalid Credentials");
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        
    }
}
