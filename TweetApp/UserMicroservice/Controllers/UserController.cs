using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UserMicroservice.Exceptions;
using UserMicroservice.Model;
using UserMicroservice.Services;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace UserMicroservice.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserServices _services;
        public UserController(IUserServices services)
        {
            _services = services;
        }
        // GET: api/<UserController>
        //[HttpGet]
        //public IEnumerable<string> Get()
        //{
        //    return new string[] { "value1", "value2" };
        //}

        // GET api/<UserController>/5
        //[HttpGet("{id}")]
        //public string Get(int id)
        //{
        //    return "value";
        //}

        // POST api/<UserController>
        [HttpGet("{email}")]
        public IActionResult Get(string email)
        {
            try
            {
                var message = _services.GetUserService(email);
                if(message==null)
                {
                    return BadRequest(message);
                }
                return Ok(message);
            }
            catch(Exception ex)
            {
                throw new CustomException(ex.Message);
            }
        }

        [HttpPost]
        public IActionResult Post([FromBody] User user)
        {
            try
            {
               var message= _services.AddUserService(user);
               return Ok(message);
            }
            catch(Exception ex)
            {
                throw new CustomException(ex.Message);
            }
        }

        // PUT api/<UserController>/5
        [HttpPut("{email}/{password}/{key}")]
        public IActionResult Put(string email,string password,string key)
        {
            try
            {
              var message=_services.ForgotPasswordService(email,password,key);
                return Ok(message);
            }
            catch (Exception ex)
            {
                throw new CustomException(ex.Message);
            }

        }

        // DELETE api/<UserController>/5
        //[HttpDelete("{id}")]
        //public void Delete(int id)
        //{
        //}
    }
}
