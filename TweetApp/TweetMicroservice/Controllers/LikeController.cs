using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TweetMicroservice.Model;
using TweetMicroservice.Repository;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TweetMicroservice.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LikeController : ControllerBase
    {
        private readonly ITweetRepository _repo;
        public LikeController(ITweetRepository repo)
        {
            _repo = repo;
        }
       

        // GET api/<LikeController>/5
        [Authorize]
        [HttpGet("{id}/{email}")]
        public IActionResult Get(int id,string email)
        {
            var message = _repo.LikeTweet(id,email); 
            if(message== "Tweet does not exist")
            {
                return BadRequest();
            }
            return Ok(message);
        }

       

        // PUT api/<LikeController>/5
        [Authorize]
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] Tweet tweet)
        {
            var message = _repo.ReplyTweet(id, tweet);
            if(message== "The tweet does not exist that you want to reply")
            {
                return BadRequest();
            }
            return Ok(message);
        }

        
    }
}
