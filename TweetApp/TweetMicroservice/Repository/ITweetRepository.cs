using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TweetMicroservice.Model;

namespace TweetMicroservice.Repository
{
   public interface ITweetRepository
    {
        public string AddTweet(Tweet tweet);
        public List<Tweet> GetAllTweets();
        public List<Tweet> ViewMyTweets(string email);
        public string UpdateTweet(int id,string tweetPost);
        public string DeleteTweet(int id);
        public List<Tweet> FindTweetByUsers(string name);   // This will call user microservice
        public string LikeTweet(int id,string email);
        public string ReplyTweet(int id,Tweet tweet);
        public List<Tweet> ViewReply(int id);

    }
}
