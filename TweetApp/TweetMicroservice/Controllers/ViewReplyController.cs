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
    public class ViewReplyController : ControllerBase
    {
        private readonly ITweetRepository _repo;
        public ViewReplyController(ITweetRepository repo)
        {
            _repo = repo;
        }
        // GET: api/<ViewReplyController>
        //[HttpGet]
        //public IEnumerable<string> Get()
        //{
        //    return new string[] { "value1", "value2" };
        //}

        // GET api/<ViewReplyController>/5
        [Authorize]
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var res = _repo.ViewReply(id);
            if (res == null)
            {
                return BadRequest("No such tweet exists");
            }
           return Ok(res);
        }

        // POST api/<ViewReplyController>
        //[HttpPost]
        //public void Post([FromBody] string value)
        //{
        //}

        // PUT api/<ViewReplyController>/5
        //[HttpPut("{id}")]
        //public void Put(int id, [FromBody] string value)
        //{
        //}

        // DELETE api/<ViewReplyController>/5
        //[HttpDelete("{id}")]
        //public void Delete(int id)
        //{
        //}
    }
}
