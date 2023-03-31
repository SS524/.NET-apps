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
    public class TweetController : ControllerBase
    {
        private readonly ITweetRepository _repo;
        public TweetController(ITweetRepository repo)
        {
            _repo = repo;
        }
        // GET: api/<TweetController>
        [Authorize]
        [HttpGet]
        public IActionResult Get()
        {
            return Ok(_repo.GetAllTweets());
        }

        // GET api/<TweetController>/5
        [Authorize]
        [HttpGet("{email}")]
        public IActionResult Get(string email)
        {
            var tweets_obj = _repo.ViewMyTweets(email);
            if (tweets_obj.Count==0)
            {
                return BadRequest();
            }
           return Ok(tweets_obj);
        }


        // POST api/<TweetController>
        [Authorize]
        [HttpPost]
        public IActionResult Post([FromBody] Tweet tweet)
        {
            string message = _repo.AddTweet(tweet);
            return Ok(message);
        }

        // PUT api/<TweetController>/5
        [Authorize]
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] string tweetPost)
        {
            var mesage = _repo.UpdateTweet(id, tweetPost);
            return Ok(mesage);
        }

        // DELETE api/<TweetController>/5
        [Authorize]
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var message = _repo.DeleteTweet(id);
            return Ok(message);
        }
    }
}
