using System;
using System.Collections.Generic;
using System.Linq;
using TweetMicroservice.Context;
using TweetMicroservice.Model;

namespace TweetMicroservice.Repository
{
    public class TweetRepository : ITweetRepository
    {
        private readonly DataContext _context;
        public TweetRepository(DataContext context)
        {
            _context = context;
        }
        public string AddTweet(Tweet tweet)
        {
            try
            {
                if (string.IsNullOrEmpty(tweet.Post))
                {
                    return "Post can not be null";
                }
                _context.Add(tweet);
                _context.SaveChanges();
                return "Your tweet has been posted";
            }
            catch(Exception ex)
            {
                return ex.Message;
            }
        }

        public string DeleteTweet(int id)
        {
            try
            {
                Tweet tweet = _context.TweetTbl.Find(id);  //ADDITIONAL WORK REQUIRED TO DELETE ON REPLY TWEET
                if (tweet == null)
                {
                    return "Your Tweet does not exist";
                }
          
                //tweets.DeleteOne(x => x.Id == id);
                _context.TweetTbl.Remove(tweet);
                _context.SaveChanges();
                foreach(var item in _context.TweetTbl.ToList())
                {
                    if (item.ReplyID == id)
                    {
                        DeleteTweet(item.Id);
                    }
                }
                return "Your tweet has been deleted";
            }
            catch(Exception ex)
            {
                return ex.Message;
            }
        }

        public List<Tweet> FindTweetByUsers(string name)
        {
            List<Tweet> tweet_list = new List<Tweet>();
            
            foreach (var item in _context.TweetTbl.ToList())
            {
                if (item.PostedBy.Contains(name))
                {
                    tweet_list.Add(item);
                }
                
            }
            return tweet_list;
        }

        public List<Tweet> GetAllTweets()
        {
            return _context.TweetTbl.ToList();
        }

        public string LikeTweet(int id,string email)
        {

            try
            {
                var obj = _context.TweetTbl.Find(id);
                if (obj == null)
                {
                    return "Tweet does not exist";
                }
                obj.LikeCount++;
                obj.LikedBy = obj.LikedBy + email + ",";
                int ct = 0;
                string[] words = obj.LikedBy.Split(',');
                foreach (var word in words)
                {
                    if(word== email)
                    {
                        ct++;
                    }
                }
                if(ct>1)
                {
                    obj.LikeCount--;
                    obj.LikeCount--;
                    obj.LikedBy=obj.LikedBy.Remove(obj.LikedBy.Length-email.Length-1);
                    email = email + ",";
                    obj.LikedBy = obj.LikedBy.Replace(email,"");
                    _context.SaveChanges();

                    return "You disliked this tweet";
                }
                //bj.LikeCount++;
                _context.SaveChanges();

                return "You liked this tweet";
            }
            catch(Exception ex)
            {
                return ex.Message;
            }
        }

        public string ReplyTweet(int id, Tweet tweet)
        {
            try
            {
                var obj = _context.TweetTbl.Find(id);
                if (obj == null)
                {
                    return "The tweet does not exist that you want to reply";
                }
                tweet.ReplyID = obj.Id;
                _context.Add(tweet);
                _context.SaveChanges();
                return "You have replied to the tweet";

            }
            catch(Exception ex)
            {
                return ex.Message;
            }
        }

        public string UpdateTweet(int id,string tweetPost)
        {
            try
            {
                var obj = _context.TweetTbl.Find(id);
                if (obj == null)
                {
                    return "Your tweet does not exist";
                }
                obj.Post = tweetPost;
                _context.TweetTbl.Find(obj.Id).Post = tweetPost;
                _context.SaveChanges();
                //tweets.FindOneAndUpdate<Tweet>(Builders<Tweet>.Filter.Eq("Id",obj.Id),
                //   Builders<Tweet>.Update.Set("Post", obj.Post));
                return "Your tweet has been updated";
            }
            catch(Exception ex)
            {
                return ex.Message;
            }
        }

        public List<Tweet> ViewMyTweets(string email)
        {
            List<Tweet> mytweets = new List<Tweet>();
            foreach(var x in _context.TweetTbl.ToList())
            {
                if (x.PostedBy == email)
                {
                    mytweets.Add(x);
                }
            }
            return mytweets;
        }

        public List<Tweet> ViewReply(int id)
        {
            try
            {
                List<Tweet> tls = new List<Tweet>();
                var obj = _context.TweetTbl.Find(id);
                if (obj == null) {
                    return null;
                }
                foreach(var item in _context.TweetTbl.ToList())
                {
                    if (item.ReplyID == obj.Id)
                    {
                        tls.Add(item);
                    }
                }
                return tls;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
