using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TweetMicroservice.Repository;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TweetMicroservice.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FindController : ControllerBase
    {
        private readonly ITweetRepository _repo;
        public FindController(ITweetRepository repo)
        {
            _repo = repo;
        }
        // GET: api/<FindController>
        [Authorize]
        [HttpGet("{name}")]
        public IActionResult Get(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                return BadRequest("Name can not be null");
            }
            var obj = _repo.FindTweetByUsers(name);
            //if (obj.Count==0)
            //{
            //    return BadRequest("No Tweets have been posted by such user name");
            //}
            return Ok(obj);
        }

        // GET api/<FindController>/5
        //[HttpGet("{id}")]
        //public string Get(int id)
        //{
        //    return "value";
        //}

        // POST api/<FindController>
        //[HttpPost]
        //public void Post([FromBody] string value)
        //{
        //}

        // PUT api/<FindController>/5
        //[HttpPut("{id}")]
        //public void Put(int id, [FromBody] string value)
        //{
        //}

        // DELETE api/<FindController>/5
        //[HttpDelete("{id}")]
        //public void Delete(int id)
        //{
        //}
    }
}
