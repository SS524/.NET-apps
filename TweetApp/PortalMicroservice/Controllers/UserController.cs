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
    public class UserController : ControllerBase
    {
        private IConfiguration Configuration;
        private IValidations _validations;
        Uri baseAddress;
        HttpClient client;
        public UserController(IConfiguration configuration,IValidations validations)
        {
            _validations = validations;
            Configuration = configuration;
            baseAddress = new Uri(Configuration["Links:UserMicroservice"]);
            client = new HttpClient();
            client.BaseAddress = baseAddress;
        }
        

        // POST api/<UserController>
        [HttpPost]
        public IActionResult Post([FromBody] User user)
        {
            try
            {
                string token = HttpContext.Request.Cookies["token"];
                if (token != null)
                {
                    return Ok("You can not add user without logout");
                }
                string validateMessage = ValidateUser(user.Email, user.Password, user.DateOfBirth);
                if(validateMessage!= "Data validated")
                {
                    return Ok(validateMessage);
                }
                string data = JsonConvert.SerializeObject(user);
                StringContent content = new StringContent(data, Encoding.UTF8, "application/json");
                HttpResponseMessage response = client.PostAsync(client.BaseAddress, content).Result;
                if (response.IsSuccessStatusCode)
                {
                    string message = response.Content.ReadAsStringAsync().Result;
                    return Ok(message);
                }
                return BadRequest();
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        // PUT api/<UserController>/5
        [HttpPut]
        public IActionResult Put([FromBody] ForgotPasswordDto forgotPasswordDto)
        {
            try
            {
                string token = HttpContext.Request.Cookies["token"];
                if (token != null)
                {
                    return Ok("You can not reset user password without logout");
                }
                if (!_validations.EmailValidation(forgotPasswordDto.Email))
                {
                    return Ok("Please provide valid email address");
                }
                if (!_validations.PasswordValidation(forgotPasswordDto.Password))
                {
                    return Ok("Please provide valid password");
                }
                if (!_validations.EmailValidation(forgotPasswordDto.Email) && !_validations.PasswordValidation(forgotPasswordDto.Password))
                {
                    return Ok("Please provide valid email and password");
                }
                string data = JsonConvert.SerializeObject(forgotPasswordDto);
                StringContent content = new StringContent(data, Encoding.UTF8, "application/json");
                HttpResponseMessage response = client.PutAsync(client.BaseAddress + "/" + forgotPasswordDto.Email + "/" + forgotPasswordDto.Password+"/"+forgotPasswordDto.SecretKey, content).Result;
                if (response.IsSuccessStatusCode)
                {
                    string message = response.Content.ReadAsStringAsync().Result;
                    return Ok(message);
                }
                return BadRequest();
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        
        private string ValidateUser(string email,string password,string date)
        {
            if(!_validations.EmailValidation(email) && _validations.PasswordValidation(password) && _validations.DateValidation(date))
            {
                return "Provide valid email address";
            }
            if(_validations.EmailValidation(email) && !_validations.PasswordValidation(password) && _validations.DateValidation(date))
            {
                return "Provide valid password";
            }
            if (_validations.EmailValidation(email) && _validations.PasswordValidation(password) && !_validations.DateValidation(date))
            {
                return "Please provide date in valid format: DD-MM-YYYY";
            }
            if (!_validations.EmailValidation(email) && !_validations.PasswordValidation(password) && _validations.DateValidation(date))
            {
                return "Please provide valid email and password";
            }
            if (!_validations.EmailValidation(email) && _validations.PasswordValidation(password) && !_validations.DateValidation(date))
            {
                return "Please provide valid email and date in valid format: DD-MM-YYYY";
            }
            if (_validations.EmailValidation(email) && !_validations.PasswordValidation(password) && !_validations.DateValidation(date))
            {
                return "Please provide valid password and  date in valid format: DD-MM-YYYY";
            }
            if (!_validations.EmailValidation(email) && !_validations.PasswordValidation(password) && !_validations.DateValidation(date))
            {
                return "Please provide valid email,valid password and date in valid format: DD-MM-YYYY";
            }
            return "Data validated";
        }
    }
}
